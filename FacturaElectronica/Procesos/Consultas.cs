using AccesoDatos;
using FacturaElectronica.Documento;
using FacturaElectronica.SRI;
using FacturaElectronica.Tools;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Xml.Schema;
using System.Xml.Linq;

namespace FacturaElectronica.Clases
{
    public static class Consultas
    {

        public static List<DocumentoElectronico> GetListFacturas(
            string id, string table, string tableDetalle, string tablePago, Directorio directorio, string codigoIva, bool crearClaveAcceso)
        {
            Resultado result = new Resultado();
            Resultado pathDestino = directorio.Path(EstadoDocumento.SinFirma);

            if (pathDestino.Estado)
            {
                List<DocumentoElectronico> documentos = new List<DocumentoElectronico>();
                string sqlInformation = Queries.SelectFacturaGeneric(id, table, tablePago);

                using (DataTable information = SqlServer.EXEC_SELECT(sqlInformation))
                {

                    foreach (DataRow inf in information.Rows)
                    {
                        DocumentoElectronico documento = new DocumentoElectronico();
                        documento.Tipo = TipoDocumento.Factura;
                        documento.Estado = EstadoDocumento.SinFirma;
                        documento.certificado = inf["certificado"].ToString();
                        documento.clave = inf["clave"].ToString();
                        documento.Table = table;
                        documento.Id = Convert.ToInt64(inf["idFactura"].ToString());

                        if (String.IsNullOrEmpty(inf["certificado"].ToString()))
                            throw new Exception($"No existe el certificado en la base de datos del ruc: {inf["ruc"]}");
                        if (String.IsNullOrEmpty(inf["clave"].ToString()))
                            throw new Exception($"No existe una clave del certificado en la base de datos del ruc: {inf["ruc"]}");
                        if (String.IsNullOrEmpty(inf["ambiente"].ToString()))
                            throw new Exception($"No existe un ambiente en la factura: {documento.Nombre}");
                        if (string.IsNullOrEmpty(inf["claveAcceso"].ToString()) && !crearClaveAcceso)
                            throw new Exception($"No existe la clave de acceso en la factura: {documento.Nombre}");

                        factura fact = new factura();

                        fact.infoTributaria = new facturaInfoTributaria()
                        {
                            ambiente = Convert.ToInt32(inf["ambiente"].ToString()),
                            tipoEmision = 1, // Tabla 2
                            razonSocial = inf["razonSocial"].ToString(),
                            nombreComercial = inf["nombreComercial"].ToString(),
                            ruc = inf["ruc"].ToString(),
                            codDoc = inf["codDoc"].ToString(), // Tabla 3
                            estab = inf["estab"].ToString(),
                            ptoEmi = inf["ptoEmi"].ToString(),
                            secuencial = inf["secuencial"].ToString().PadLeft(9, '0'),
                            dirMatriz = inf["dirMatriz"].ToString(),
                            agenteRetencion = inf["agenteRetencion"].ToString() == "" ? "0" : inf["agenteRetencion"].ToString()
                        };

                        //regimenMicroempresas solo se agregará si tiene datos
                        if (!string.IsNullOrEmpty(inf["regimenMicroempresas"].ToString()))
                            fact.infoTributaria.regimenMicroempresas = inf["regimenMicroempresas"].ToString();

                        fact.infoFactura = new facturaInfoFactura()
                        {
                            fechaEmision = inf["fechaEmision"].ToString(),
                            dirEstablecimiento = inf["dirEstablecimiento"].ToString(),
                            contribuyenteEspecial = inf["contribuyenteEspecial"].ToString() == "" ? "000" : inf["contribuyenteEspecial"].ToString(),
                            obligadoContabilidad = inf["obligadoContabilidad"].ToString(),
                            tipoIdentificacionComprador = inf["tipoIdentificacionComprador"].ToString(),
                            razonSocialComprador = inf["razonSocialComprador"].ToString(),
                            direccionComprador = inf["direccionComprador"].ToString() == "" ? "SIN DIRECCIÓN" : inf["direccionComprador"].ToString(),
                            identificacionComprador = inf["identificacionComprador"].ToString(),
                            totalSinImpuestos = inf["totalSinImpuestos"].ToString().Replace(',', '.'),
                            totalDescuento = "0.0000",
                            totalConImpuestos = new List<facturaInfoFacturaTotalImpuesto>(),
                            propina = "0.0000",
                            importeTotal = inf["importeTotal"].ToString().Replace(',', '.'),
                            moneda = "DOLAR"
                        };

                        // Si el parámetro de crear Clave de Acceso está habilitado
                        if (crearClaveAcceso)
                        {
                            fact.infoTributaria.claveAcceso = Calcs.GetClaveAcceso(
                            fact.infoFactura.fechaEmision,
                            fact.infoTributaria.codDoc,
                            fact.infoTributaria.ruc,
                            fact.infoTributaria.ambiente.ToString(),
                            fact.infoTributaria.estab + fact.infoTributaria.ptoEmi,
                            fact.infoTributaria.secuencial,
                            fact.infoTributaria.tipoEmision.ToString());

                            fact.infoTributaria.claveAcceso = fact.infoTributaria.claveAcceso;
                        }
                        else
                            fact.infoTributaria.claveAcceso = inf["claveAcceso"].ToString();
                        
                        documento.ClaveAcceso = fact.infoTributaria.claveAcceso;

                        //Crear Total con impuestos
                        // Permite traer la sumatoria de valores separados por tipo de impuesto.
                        string sqlTotalConImpuestos = Queries.SelectImpuestosGeneric(id, tableDetalle, documento.Id);

                        using (DataTable impuestos = SqlServer.EXEC_SELECT(sqlTotalConImpuestos))
                        {
                            foreach (DataRow imp in impuestos.Rows)
                            {
                                fact.infoFactura.totalConImpuestos.Add(new facturaInfoFacturaTotalImpuesto()
                                {
                                    codigo = codigoIva, //Falta traer este campo desde la base
                                    codigoPorcentaje = imp["codigoPorcentaje"].ToString(),
                                    baseImponible = imp["baseImponible"].ToString().Replace(',', '.'),
                                    tarifa = imp["tarifa"].ToString(),
                                    valor = imp["valor"].ToString().Replace(',', '.'),
                                });
                            }
                        }

                        // Revisar si puede enlazarse pagos en el sistema
                        fact.infoFactura.pagos = new List<facturaPago>();
                        fact.infoFactura.pagos.Add(new facturaPago()
                        {
                            formaPago = inf["formaPago"].ToString(),
                            total = inf["importeTotal"].ToString().Replace(',', '.')
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
                                facturaDetalle.cantidad = detalle["cantidad"].ToString().Replace(',', '.');
                                facturaDetalle.precioUnitario = detalle["precioUnitario"].ToString().Replace(',', '.');
                                facturaDetalle.descuento = "0.0000";
                                facturaDetalle.precioTotalSinImpuesto = detalle["precioTotalSinImpuesto"].ToString().Replace(',', '.');
                                facturaDetalle.impuestos = new facturaDetalleImpuestos();
                                facturaDetalle.impuestos.impuesto = new facturaDetalleImpuestosImpuesto();
                                facturaDetalle.impuestos.impuesto.codigo = codigoIva;
                                facturaDetalle.impuestos.impuesto.codigoPorcentaje = detalle["codigoPorcentaje"].ToString();
                                facturaDetalle.impuestos.impuesto.tarifa = detalle["tarifa"].ToString();
                                facturaDetalle.impuestos.impuesto.baseImponible = detalle["precioTotalSinImpuesto"].ToString().Replace(',', '.');
                                facturaDetalle.impuestos.impuesto.valor = detalle["valor"].ToString().Replace(',', '.');
                                fact.detalles.Add(facturaDetalle);
                            }
                        }

                        fact.infoAdicional.Add(new facturaCampoAdicional()
                        {
                            nombre = "Dirección",
                            Value = inf["direccionComprador"].ToString() == "" ? "SIN DIRECCIÓN" : inf["direccionComprador"].ToString(),
                        });
                        fact.infoAdicional.Add(new facturaCampoAdicional()
                        {
                            nombre = "Teléfono",
                            Value = string.IsNullOrEmpty(inf["Telefono"].ToString()) ? "0000000000" : inf["Telefono"].ToString()
                        });
                        fact.infoAdicional.Add(new facturaCampoAdicional()
                        {
                            nombre = "Email",
                            Value = inf["Email"].ToString() == "" ? "example@email.com" : inf["Email"].ToString()
                        });

                        // Crear xml sin firma en la carpeta seleccionada
                        System.IO.File.WriteAllText(pathDestino.Mensaje + documento.Nombre + ".xml", XmlTools.Serialize(fact));

                        //Validar con el esquema
                        result = ValidateSchema(directorio, documento.Nombre);
                        if (!result.Estado)
                            throw new Exception($"Ha ocurrido un problema al validar el xml: {result.Mensaje}");

                        if (crearClaveAcceso)
                            UpdateData(Queries.UpdateClaveAcceso(), table, documento.ClaveAcceso, id, documento.Id);

                        documento.LogoEmpresa = inf["LogoEmpresa"].ToString();
                        documentos.Add(documento);
                    }
                }

                //string xml = XmlTools.Serialize(fact);
                return documentos;
            } 
            else
                throw new Exception("Ha ocurrido un error al crear la carpeta de No Firmados");
        }

        public static string GetCodigoIva()
        {
            string sqlInformation = Queries.SelectCodigoIva();
            var a  = SqlServer.EXEC_SCALAR(sqlInformation).ToString();
            return a;
        }

        public static Resultado UpdateData(string query, params object[] args)
        {
            Resultado resultado = new Resultado();
            string sql = String.Format(query, args);
            string res = SqlServer.EXEC_COMMAND(sql);
            resultado.Estado = res == "OK";
            resultado.Mensaje = res == "OK" ? SqlServer.MensajeDeActualizar : res;

            return resultado;
        }

        private static Resultado ValidateSchema(Directorio directorio, string xmlName)
        {
            Resultado resultado = new Resultado();
            Resultado pathXsd = directorio.Path(ConfigurationManager.AppSettings["validateSchema"]);
            Resultado pathSinFirma = directorio.Path(EstadoDocumento.SinFirma);
            try
            {
                XmlSchemaSet schema = new System.Xml.Schema.XmlSchemaSet();
                schema.Add("", pathXsd.Mensaje + "factura_V1.1.0.xsd");

                XDocument xmlDocument = XDocument.Load(pathSinFirma.Mensaje + xmlName + ".xml");
                xmlDocument.Validate(schema, (o, e) =>
                {
                    resultado.Mensaje += e.Message + Environment.NewLine;
                });

                resultado.Estado = resultado.Mensaje == null;
                return resultado;
            }
            catch (Exception ex)
            {
                resultado.Estado = false;
                resultado.Mensaje = ex.Message;
            }

            return resultado;
        }
    }
}
