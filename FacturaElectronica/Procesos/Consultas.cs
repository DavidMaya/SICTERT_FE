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

        public static List<DocumentoElectronico> GetListFacturas(string id, string table, string tableDetalle, Directorio directorio)
        {
            Resultado pathDestino = directorio.Path(EstadoDocumento.SinFirma);
            if (pathDestino.Estado)
            {
                List<DocumentoElectronico> documentos = new List<DocumentoElectronico>();
                ///* 1. Traer los datos de cada factura para elaborar los xml.
                /// Aquí debe obtener los resultado de la tabla FACTURA habilitados
                /// y que no tengan el estado NULL.
                /// 

                string sqlInformation = Queries.SelectFacturaGeneric(id, table);
                using (DataTable information = SqlServer.EXEC_SELECT(sqlInformation))
                {

                    foreach (DataRow inf in information.Rows)
                    {
                        DocumentoElectronico documento = new DocumentoElectronico();
                        documento.Tipo = (TipoDocumento)Convert.ToInt32(inf["ambiente"].ToString());
                        documento.Estado = EstadoDocumento.SinFirma;
                        documento.certificado = inf["certificado"].ToString();
                        documento.clave = inf["clave"].ToString();
                        documento.Table = table;
                        documento.Id = Convert.ToInt64(inf["idFactura"].ToString());

                        factura fact = new factura();

                        fact.infoTributaria = new facturaInfoTributaria()
                        {
                            ambiente = Convert.ToSByte(inf["ambiente"].ToString()),
                            tipoEmision = 1,
                            //razonSocial = inf["razonSocial"].ToString(),
                            //ruc = inf["ruc"].ToString(),
                            codDoc = "01",
                            //estab = inf["estab"].ToString(),
                            //ptoEmi = inf["ptoEmi"].ToString(),
                            //secuencial = inf["secuencial"].ToString(),
                            //dirMatriz = inf["dirMatriz"].ToString(),

                            // Test
                            razonSocial = "MVJ-RASTER TECNOLOGIAS APLICADAS S. A.",
                            ruc = "1891760171001",
                            estab = "001",
                            ptoEmi = "001",
                            secuencial = "000000110",
                            dirMatriz = "Portoviejo 02-44 y Tulcán"
                        };

                        fact.infoFactura = new facturaInfoFactura()
                        {
                            //fechaEmision = inf["fechaEmision"].ToString(),
                            //dirEstablecimiento = inf["dirEstablecimiento"].ToString(),
                            //contribuyenteEspecial = (short)(inf["obligadoContabilidad"] == null ? 0 : Convert.ToInt16(inf["obligadoContabilidad"].ToString())),
                            contribuyenteEspecial = 0,
                            obligadoContabilidad = inf["obligadoContabilidad"].ToString(),
                            tipoIdentificacionComprador = "05",
                            razonSocialComprador = inf["razonSocialComprador"].ToString(),
                            identificacionComprador = inf["identificacionComprador"].ToString(),
                            totalSinImpuestos = float.Parse(inf["totalSinImpuestos"].ToString()),
                            totalDescuento = 0,
                            totalConImpuestos = new List<facturaInfoFacturaTotalImpuesto>(),
                            propina = 0,
                            importeTotal = float.Parse(inf["importeTotal"].ToString()),

                            // Test
                            moneda = "DOLAR",
                            fechaEmision = "21/12/2021",
                            dirEstablecimiento = "Portoviejo 02-44 y Tulcán"
                        };

                        //Crear Total con impuestos
                        // Permite traer la sumatoria de valores separados por tipo de impuesto.
                        string sqlTotalConImpuestos = Queries.SelectImpuestosGeneric(id, tableDetalle, documento.Id);

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
                        fact.infoFactura.pagos = new List<facturaPago>();
                        fact.infoFactura.pagos.Add(new facturaPago()
                        {
                            formaPago = "01",
                            importeTotal = float.Parse(inf["importeTotal"].ToString())
                        });

                        // Agregar detalles
                        string sqlDetalles = Queries.SelectDetalleGeneric(id, tableDetalle, documento.Id);

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

                        // Agregar clave de acceso a datos
                        fact.infoTributaria.claveAcceso = Calcs.GetClaveAcceso(
                            fact.infoFactura.fechaEmision,
                            fact.infoTributaria.codDoc,
                            fact.infoTributaria.ruc,
                            fact.infoTributaria.ambiente.ToString(),
                            fact.infoTributaria.estab + fact.infoTributaria.ptoEmi,
                            fact.infoTributaria.secuencial,
                            fact.infoTributaria.tipoEmision.ToString());

                        documento.xml = XmlTools.Serialize(fact);
                        // Crear xml sin firma en la carpeta seleccionada
                        System.IO.File.WriteAllText(pathDestino.Mensaje + documento.Nombre + ".xml", XmlTools.Serialize(fact)); documentos.Add(documento);
                    }
                }

                //string xml = XmlTools.Serialize(fact);
                return documentos;
            } 
            else
                throw new Exception("Ha ocurrido un error al crear la carpeta de No Firmados");
        }

        public static Resultado UpdateEstadoFactura(long id, string table, string estado)
        {
            Resultado resultado = new Resultado();

            string sql = Queries.UpdateEstadoFactura(id, table, estado);
            string res = SqlServer.EXEC_COMMAND(sql);
            resultado.Estado = res == "OK";
            resultado.Mensaje = res == "OK" ? SqlServer.MensajeDeActualizar : res;

            return resultado;
        }
    }
}
