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
using System.Linq;

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
                        fact.version = ConfigurationManager.AppSettings["versionFactura"];
                        fact.id = "comprobante";

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

                        // regimenMicroempresas solo se agregará si tiene datos
                        // Anexo 22. Se debe agregar un nuevo nodo para RIMPE.
                        if (inf["regimenCodigo"].ToString().Contains("RIMPE"))
                            fact.infoTributaria.contribuyenteRimpe = inf["regimenMicroempresas"].ToString();
                        else
                        {
                            if (!string.IsNullOrEmpty(inf["regimenMicroempresas"].ToString()))
                                fact.infoTributaria.regimenMicroempresas = inf["regimenMicroempresas"].ToString();
                        }

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
                                    codigo = codigoIva,
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
                        result = ValidateSchema(directorio, documento.Nombre, fact.version, "factura_V");
                        //if (!result.Estado)
                        //    throw new Exception($"Ha ocurrido un problema al validar el xml: {result.Mensaje}");

                        if (crearClaveAcceso)
                            SqlServer.UpdateData(Queries.UpdateClaveAcceso(), table, documento.ClaveAcceso, id, documento.Id);

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


        public static List<DocumentoElectronico> GetListNotasCredito(
            string idNotaCredito, string idFactura, string table, string tableFactura, string tableDetalle,
            string tablePago, Directorio directorio, string codigoIva, bool crearClaveAcceso)
        {
            Resultado result = new Resultado();
            Resultado pathDestino = directorio.Path(EstadoDocumento.SinFirma);

            if (pathDestino.Estado)
            {
                List<DocumentoElectronico> documentos = new List<DocumentoElectronico>();
                string sqlInformation = Queries.SelectNotaCredito(idNotaCredito, idFactura, table, tableFactura, tablePago);

                using (DataTable information = SqlServer.EXEC_SELECT(sqlInformation))
                {
                    foreach (DataRow inf in information.Rows)
                    {
                        DocumentoElectronico documento = new DocumentoElectronico();
                        documento.Tipo = TipoDocumento.NotaCredito;
                        documento.Estado = EstadoDocumento.SinFirma;
                        documento.certificado = inf["certificado"].ToString();
                        documento.clave = inf["clave"].ToString();
                        documento.Table = table;
                        documento.Id = Convert.ToInt64(inf["idNotaCredito"].ToString());

                        if (String.IsNullOrEmpty(inf["certificado"].ToString()))
                            throw new Exception($"No existe el certificado en la base de datos del ruc: {inf["ruc"]}");
                        if (String.IsNullOrEmpty(inf["clave"].ToString()))
                            throw new Exception($"No existe una clave del certificado en la base de datos del ruc: {inf["ruc"]}");
                        if (String.IsNullOrEmpty(inf["ambiente"].ToString()))
                            throw new Exception($"No existe un ambiente en la factura: {documento.Nombre}");
                        if (string.IsNullOrEmpty(inf["claveAcceso"].ToString()) && !crearClaveAcceso)
                            throw new Exception($"No existe la clave de acceso en la factura: {documento.Nombre}");

                        notaCredito notaCredito = new notaCredito();
                        notaCredito.version = ConfigurationManager.AppSettings["versionFactura"];
                        notaCredito.id = "comprobante";

                        notaCredito.infoTributaria = new InfoTributaria()
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

                        // regimenMicroempresas solo se agregará si tiene datos
                        // Anexo 22. Se debe agregar un nuevo nodo para RIMPE.
                        if (inf["regimenCodigo"].ToString().Contains("RIMPE"))
                            notaCredito.infoTributaria.contribuyenteRimpe = inf["regimenMicroempresas"].ToString();
                        else
                        {
                            if (!string.IsNullOrEmpty(inf["regimenMicroempresas"].ToString()))
                                notaCredito.infoTributaria.regimenMicroempresas = inf["regimenMicroempresas"].ToString();
                        }

                        notaCredito.infoNotaCredito = new InfoNotaCredito()
                        {
                            fechaEmision = inf["fechaEmision"].ToString(),
                            dirEstablecimiento = inf["dirEstablecimiento"].ToString(),
                            tipoIdentificacionComprador = inf["tipoIdentificacionComprador"].ToString(),
                            razonSocialComprador = inf["razonSocialComprador"].ToString(),
                            identificacionComprador = inf["identificacionComprador"].ToString(),
                            contribuyenteEspecial = inf["contribuyenteEspecial"].ToString() == "" ? "000" : inf["contribuyenteEspecial"].ToString(),
                            obligadoContabilidad = inf["obligadoContabilidad"].ToString(),
                            //rise = "",
                            //codDocModificado = inf["secuencialFac"].ToString(),
                            codDocModificado = "00",
                            numDocModificado = $"{inf["estabFac"]}-{inf["ptoEmiFac"]}-{inf["secuencialFac"].ToString().PadLeft(9, '0')}",
                            fechaEmisionDocSustento = inf["fechaEmisionDocSustento"].ToString(),
                            totalSinImpuestos = inf["totalSinImpuestos"].ToString().Replace(',', '.'),
                            // Revisar compensaciones
                            //compensaciones = new List<Compensacion>(),
                            valorModificacion = inf["totalSinImpuestos"].ToString().Replace(',', '.'),
                            moneda = "DOLAR",
                            totalConImpuestos = new List<TotalImpuesto>(),
                            motivo = "motivo0"
                        };

                        // Si el parámetro de crear Clave de Acceso está habilitado
                        if (crearClaveAcceso)
                        {
                            notaCredito.infoTributaria.claveAcceso = Calcs.GetClaveAcceso(
                            notaCredito.infoNotaCredito.fechaEmision,
                            notaCredito.infoTributaria.codDoc,
                            notaCredito.infoTributaria.ruc,
                            notaCredito.infoTributaria.ambiente.ToString(),
                            notaCredito.infoTributaria.estab + notaCredito.infoTributaria.ptoEmi,
                            notaCredito.infoTributaria.secuencial,
                            notaCredito.infoTributaria.tipoEmision.ToString());

                            notaCredito.infoTributaria.claveAcceso = notaCredito.infoTributaria.claveAcceso;
                        }
                        else
                            notaCredito.infoTributaria.claveAcceso = inf["claveAcceso"].ToString();

                        // Crear compesaciones
                        //string sqlCompensaciones = Queries.SelectNotaCreditoCompensacion(idNotaCredito, documento.Id, tableDetalle);
                        //using (DataTable compensaciones = SqlServer.EXEC_SELECT(sqlCompensaciones))
                        //{
                        //    notaCredito.infoNotaCredito.
                        //        compensaciones.AddRange(
                        //        from DataRow row in compensaciones.Rows
                        //        select new Compensacion()
                        //        {
                        //            codigo = row["codigo"].ToString(),
                        //            tarifa = row["tarifa"].ToString(),
                        //            valor = row["valor"].ToString(),
                        //        });
                        //}

                        //Crear Total con impuestos
                        string sqlImpuestos = Queries.SelectNotaCreditoImpuesto(idNotaCredito, tableDetalle, documento.Id);
                        using (DataTable impuestos = SqlServer.EXEC_SELECT(sqlImpuestos))
                        {
                            //notaCredito.infoNotaCredito.totalConImpuestos.AddRange(
                            //    from DataRow row in impuestos.Rows
                            //    select new TotalImpuesto()
                            //    {
                            //        codigo = row["codigo"].ToString(),
                            //        codigoPorcentaje = row["codigoPorcentaje"].ToString(),
                            //        baseImponible = row["baseImponible"].ToString(),
                            //        valorDevolucionIva = row["valorDevolucionIva"].ToString(),
                            //        valor = row["valor"].ToString()
                            //    });
                            foreach (DataRow row in impuestos.Rows)
                            {
                                notaCredito.infoNotaCredito.totalConImpuestos.Add(new TotalImpuesto()
                                {
                                    codigo = codigoIva,
                                    codigoPorcentaje = row["codigoPorcentaje"].ToString(),
                                    baseImponible = row["baseImponible"].ToString().Replace(',', '.'),
                                    valorDevolucionIva = row["valorDevolucionIva"].ToString().Replace(',', '.'),
                                    valor = row["valor"].ToString().Replace(',', '.'),
                                });

                                //TotalImpuesto totalImpuesto = new TotalImpuesto();
                                //totalImpuesto.codigo = row["codigo"].ToString();
                                //totalImpuesto.codigoPorcentaje = row["codigoPorcentaje"].ToString();
                                //totalImpuesto.baseImponible = row["baseImponible"].ToString();
                                //totalImpuesto.valorDevolucionIva = row["valorDevolucionIva"].ToString();
                                //totalImpuesto.valor = row["valor"].ToString();
                                //notaCredito.infoNotaCredito.totalConImpuestos.Add(totalImpuesto);
                            }
                        }

                        //Crear detalles
                        string sqlDetalles = Queries.SelectNotaCreditoDetalle(idNotaCredito, tableDetalle, documento.Id);
                        notaCredito.detalles = new List<Detalle>();

                        using (DataTable detalles = SqlServer.EXEC_SELECT(sqlDetalles))
                        {
                            foreach (DataRow row in detalles.Rows)
                            {
                                Detalle detalle = new Detalle();
                                detalle.codigoInterno = row["codigoInterno"].ToString();
                                detalle.codigoAdicional = row["codigoInterno"].ToString();
                                detalle.descripcion = row["descripcion"].ToString();
                                detalle.cantidad = row["cantidad"].ToString().Replace(',', '.');
                                detalle.precioUnitario = row["precioUnitario"].ToString().Replace(',', '.');
                                detalle.descuento = "0.0000";
                                detalle.precioTotalSinImpuesto = row["precioTotalSinImpuesto"].ToString().Replace(',', '.');
                                detalle.detallesAdicionales = new List<DetAdicional>();
                                detalle.detallesAdicionales.Add(new DetAdicional()
                                {
                                    nombre = row["codigoInterno"].ToString(),
                                    valor = row["precioTotalSinImpuesto"].ToString().Replace(',', '.')
                                });
                                detalle.impuestos = new List<Impuestos>();
                                detalle.impuestos.Add(new Impuestos()
                                {
                                    codigo = codigoIva,
                                    codigoPorcentaje = row["tarifa"].ToString(),
                                    baseImponible = row["precioTotalSinImpuesto"].ToString().Replace(',', '.'),
                                    tarifa = row["precioTotalSinImpuesto"].ToString().Replace(',', '.'),
                                    valor = row["valor"].ToString().Replace(',', '.'),
                                });
                                notaCredito.detalles.Add(detalle);
                            }
                        }

                        notaCredito.infoAdicional.Add(new InfoAdicional()
                        {
                            nombre = "Dirección",
                            Value = inf["direccionComprador"].ToString() == "" ? "SIN DIRECCIÓN" : inf["direccionComprador"].ToString(),
                        });
                        notaCredito.infoAdicional.Add(new InfoAdicional()
                        {
                            nombre = "Teléfono",
                            Value = string.IsNullOrEmpty(inf["Telefono"].ToString()) ? "0000000000" : inf["Telefono"].ToString()
                        });
                        notaCredito.infoAdicional.Add(new InfoAdicional()
                        {
                            nombre = "Email",
                            Value = inf["Email"].ToString() == "" ? "example@email.com" : inf["Email"].ToString()
                        });

                        // Crear xml sin firma en la carpeta seleccionada
                        System.IO.File.WriteAllText(pathDestino.Mensaje + documento.Nombre + ".xml", XmlTools.Serialize(notaCredito));

                        //Validar con el esquema
                        result = ValidateSchema(directorio, documento.Nombre, "1.0.0", "notaCredito_V");
                        if (!result.Estado)
                            throw new Exception($"Ha ocurrido un problema al validar el xml: {result.Mensaje}");

                        //if (crearClaveAcceso)
                        //    SqlServer.UpdateData(Queries.UpdateClaveAcceso(), table, documento.ClaveAcceso, idNotaCredito, documento.Id);

                        documento.LogoEmpresa = inf["LogoEmpresa"].ToString();
                        documentos.Add(documento);
                    }
                }

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

        //public static Resultado UpdateData(string query, params object[] args)
        //{
        //    Resultado resultado = new Resultado();
        //    string sql = String.Format(query, args);
        //    string res = SqlServer.EXEC_COMMAND(sql);
        //    resultado.Estado = res == "OK";
        //    resultado.Mensaje = res == "OK" ? SqlServer.MensajeDeActualizar : res;

        //    return resultado;
        //}

        private static Resultado ValidateSchema(Directorio directorio, string xmlName, string versionXsd, string file)
        {
            Resultado resultado = new Resultado();
            Resultado pathXsd = directorio.Path(ConfigurationManager.AppSettings["validateSchema"]);
            Resultado pathSinFirma = directorio.Path(EstadoDocumento.SinFirma);
            try
            {
                XmlSchemaSet schema = new System.Xml.Schema.XmlSchemaSet();
                schema.Add("", pathXsd.Mensaje + $"{file}{versionXsd}.xsd");

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
