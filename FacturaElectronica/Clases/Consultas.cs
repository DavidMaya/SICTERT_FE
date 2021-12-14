using AccesoDatos;
using FacturaElectronica.Documento;
using FacturaElectronica.SRI;
using FacturaElectronica.Tools;
using System;
using System.Collections.Generic;
using System.Data;

namespace FacturaElectronica.Clases
{
    public static class Consultas
    {

        public static List<DocumentoElectronico> GetListFacturas()
        {
            List<DocumentoElectronico> documentos = new List<DocumentoElectronico>();
            ///* 1. Traer los datos de cada factura para elaborar los xml.
            /// Aquí debe obtener los resultado de la tabla FACTURA habilitados
            /// y que no tengan el estado NULL.
            /// 

            string sqlInformation = "SELECT " +
                // infoTributaria
                "f.Id_factura_recauda AS idFactura, " +
                "f.Id_TipoAmbiente AS ambiente, " +
                "1 AS tipoEmision, " +
                "UPPER(f.RazonSocial_Emisor) razonSocial, " +
                "f.RUC_Emisor ruc, " +
                "'0' claveAcceso, " +
                "'01' codDoc, " +
                "f.Serie AS estab, " +
                "f.CodEstablecimiento_Emisor ptoEmi, " +
                "f.Numero secuencial, " +
                "UPPER(f.DirMatriz_Emisor) dirMatriz," +
                // infoFactura
                "FORMAT(f.Fecha_Hora, 'dd/mm/yyyy') fechaEmision, " +
                "UPPER(f.DirEstablecimiento_Emisor) dirEstablecimiento, " +
                "f.ResolContribEsp_Emisor AS contribuyenteEspecial, " +
                "IIF(f.ObligadoContab_Emisor = 1, 'SI', 'NO') AS obligadoContabilidad, " +
                "f.Razon_Social AS razonSocialComprador, " +
                "f.CI_Ruc AS identificacionComprador, " +
                "CONVERT(DECIMAL(10, 4), (f.Valor + f.ValorBaseIVA)) AS totalSinImpuestos, " +
                "'0' totalDescuento, " +
                "CONVERT(DECIMAL(10,4), f.Valor_Total) AS importeTotal, " +
                // infoAdicional
                "cf.Direccion, cf.Telefono, cf.Email " +
                // table
                "FROM FACTURA_RECAUDA f " +
                "INNER JOIN CLIENTE_FINAL cf ON cf.Id_Cliente_Final = f.Codigo_TipoIdCliente " +
                // filtro que cumpla la tabla para facturación electrónica
                "WHERE f.Id_factura_recauda = 30032";
            
            using (DataTable information = SqlServer.EXEC_SELECT(sqlInformation))
            {

                foreach (DataRow inf in information.Rows)
                {
                    DocumentoElectronico documento = new DocumentoElectronico();
                    documento.Tipo = (TipoDocumento)Convert.ToInt32(inf["ambiente"].ToString());
                    documento.Estado = EstadoDocumento.SinFirma;

                    factura fact = new factura();
                    fact.version = "1.1.0";
                    fact.id = "comprobante";

                    long idFactura = Convert.ToInt64(inf["idFactura"].ToString());
                    fact.infoTributaria = new facturaInfoTributaria()
                    {
                        ambiente = Convert.ToSByte(inf["ambiente"].ToString()),
                        tipoEmision = Convert.ToSByte(inf["tipoEmision"].ToString()),
                        razonSocial = inf["razonSocial"].ToString(),
                        ruc = inf["ruc"].ToString(),
                        codDoc = inf["codDoc"].ToString(),
                        estab = inf["estab"].ToString(),
                        ptoEmi = inf["ptoEmi"].ToString(),
                        secuencial = inf["secuencial"].ToString(),
                        dirMatriz = inf["dirMatriz"].ToString(),
                    };

                    fact.infoFactura = new facturaInfoFactura()
                    {
                        fechaEmision = inf["fechaEmision"].ToString(),
                        dirEstablecimiento = inf["dirEstablecimiento"].ToString(),
                        //contribuyenteEspecial = Convert.ToInt16(table.Rows[0]["contribuyenteEspecial"].ToString()),
                        obligadoContabilidad = inf["obligadoContabilidad"].ToString(),
                        //tipoIdentificacionComprador = table.Rows[0]["obligadoContabilidad"].ToString(),
                        razonSocialComprador = inf["razonSocialComprador"].ToString(),
                        identificacionComprador = inf["identificacionComprador"].ToString(),
                        totalSinImpuestos = float.Parse(inf["totalSinImpuestos"].ToString()),
                        totalDescuento = 0,
                        totalConImpuestos = new List<facturaInfoFacturaTotalImpuesto>(),
                        propina = 0,
                        importeTotal = float.Parse(inf["importeTotal"].ToString()),
                        moneda = "DOLAR"
                    };

                    //Crear Total con impuestos
                    // Permite traer la sumatoria de valores separados por tipo de impuesto.
                    string sqlTotalConImpuestos = "SELECT " +
                        "CASE WHEN dfr.Iva = 0 THEN 0 WHEN dfr.Iva = 12 THEN 2 END AS codigo, " +
                        "CASE WHEN dfr.Iva = 0 THEN 0 WHEN dfr.Iva = 12 THEN 2 " +
                        "WHEN dfr.Iva = 14 THEN 3 END AS codigoPorcentaje, " +
                        "CONVERT(DECIMAL(10, 4), SUM(dfr.Valor * dfr.Cantidad)) AS baseImponible " +
                        "FROM DETALLE_FACT_RECAUDA dfr " +
                        $"WHERE dfr.Id_factura_recauda = ${idFactura} " +
                        "GROUP BY dfr.Iva";

                    fact.infoFactura.totalConImpuestos = new List<facturaInfoFacturaTotalImpuesto>();
                    using (DataTable impuestos = SqlServer.EXEC_SELECT(sqlTotalConImpuestos))
                    {
                        foreach (DataRow imp in impuestos.Rows)
                        {
                            fact.infoFactura.totalConImpuestos.Add(new facturaInfoFacturaTotalImpuesto()
                            {
                                codigo = imp["codigo"].ToString(),
                                codigoPorcentaje = imp["codigoPorcentaje"].ToString(),
                                baseImponible = float.Parse(imp["baseImponible"].ToString())
                            });
                        } 
                    }

                    // Revisar si puede enlazarse pagos en el sistema
                    fact.pagos = new List<facturaPago>();
                    fact.pagos.Add(new facturaPago()
                    {
                        formaPago = "01",
                        importeTotal = float.Parse(inf["importeTotal"].ToString())
                    });

                    // Agregar detalles
                    string sqlDetalles = "SELECT " +
                        "dfr.Id_Concepto_Cuenta AS codigoPrincipal, " +
                        "dfr.Nombre AS descripcion, " +
                        "dfr.Cantidad AS cantidad, " +
                        "CONVERT(DECIMAL(10, 4), dfr.Valor) AS precioUnitario, " +
                        "CONVERT(DECIMAL(10, 4), dfr.Valor * dfr.Cantidad) AS precioTotalSinImpuesto, " +
                        "CASE WHEN dfr.Iva = 0 THEN 0 WHEN dfr.Iva = 12 THEN 2 END AS codigo, " +
                        "CASE WHEN dfr.Iva = 0 THEN 0 WHEN dfr.Iva = 12 THEN 2 " +
                        "WHEN dfr.Iva = 14 THEN 3 END AS codigoPorcentaje, " +
                        "dfr.Iva AS tarifa, " +
                        "dfr.Iva_Valor AS valor " + //Aquí puede haber un cambio en el Iva
                        "FROM DETALLE_FACT_RECAUDA dfr " +
                        $"WHERE dfr.Id_factura_recauda = ${idFactura}";

                    fact.detalles = new List<facturaDetalle>();
                    using (DataTable detalles = SqlServer.EXEC_SELECT(sqlDetalles))
                    {
                        foreach (DataRow detalle in detalles.Rows)
                        {
                            facturaDetalle facturaDetalle = new facturaDetalle();
                            facturaDetalle.codigoPrincipal = detalle["codigoPrincipal"].ToString();
                            facturaDetalle.codigoAuxiliar = detalle["codigoPrincipal"].ToString();
                            facturaDetalle.descripcion = detalle["descripcion"].ToString();
                            facturaDetalle.cantidad = float.Parse(detalle["cantidad"].ToString());
                            facturaDetalle.precioUnitario = detalle["precioUnitario"].ToString();
                            facturaDetalle.descuento = 0;
                            facturaDetalle.precioTotalSinImpuesto = float.Parse(detalle["precioTotalSinImpuesto"].ToString());
                            facturaDetalle.impuestos = new facturaDetalleImpuestos();
                            facturaDetalle.impuestos.impuesto = new facturaDetalleImpuestosImpuesto();
                            facturaDetalle.impuestos.impuesto.codigo = detalle["codigo"].ToString();
                            facturaDetalle.impuestos.impuesto.codigoPorcentaje = detalle["codigoPorcentaje"].ToString();
                            facturaDetalle.impuestos.impuesto.tarifa = float.Parse(detalle["tarifa"].ToString());
                            facturaDetalle.impuestos.impuesto.baseImponible = float.Parse(detalle["precioTotalSinImpuesto"].ToString());
                            facturaDetalle.impuestos.impuesto.valor = float.Parse(detalle["valor"].ToString());
                            fact.detalles.Add(facturaDetalle);
                        }
                    }

                    fact.infoAdicional = new List<facturaCampoAdicional>();
                    fact.infoAdicional.Add(new facturaCampoAdicional()
                    {
                        nombre = "Dirección",
                        Value = inf["Direccion"].ToString()
                    });
                    fact.infoAdicional.Add(new facturaCampoAdicional()
                    {
                        nombre = "Teléfono",
                        Value = inf["Telefono"].ToString()
                    });
                    fact.infoAdicional.Add(new facturaCampoAdicional()
                    {
                        nombre = "Email",
                        Value = inf["Email"].ToString()
                    });

                    //Agregar clave de acceso a datos
                    fact.infoTributaria.claveAcceso = Calcs.GetClaveAcceso(
                        fact.infoFactura.fechaEmision,
                        fact.infoTributaria.codDoc,
                        fact.infoTributaria.ruc,
                        fact.infoTributaria.ambiente.ToString(),
                        fact.infoTributaria.estab + fact.infoTributaria.ptoEmi,
                        fact.infoTributaria.secuencial,
                        fact.infoTributaria.tipoEmision.ToString());

                    documento.xml = XmlTools.Serialize(fact);
                    documentos.Add(documento);
                }
            }

            //string xml = XmlTools.Serialize(fact);
            return documentos;
        }
    }
}
