using System;
using System.Linq;
using java.security;
using java.io;
using java.util;
using java.security.cert;
using javax.xml.parsers;
using es.mityc.javasign.pkstore;
using es.mityc.javasign.pkstore.keystore;
using es.mityc.javasign.trust;
using es.mityc.javasign.xml.xades.policy;
using es.mityc.firmaJava.libreria.xades;
using es.mityc.javasign.xml.refs;
using es.mityc.firmaJava.libreria.utilidades;
using org.w3c.dom;
using System.Net;
using System.Threading;
using sviudes.blogspot.com;
using FacturaElectronica.Documento;
using FacturaElectronica.Tools;
using FacturaElectronica.SRI;
using FacturaElectronica.Clases;
using System.Xml;

namespace FacturaElectronica.Procesos
{
    public static class Actions
    {
        public static Resultado ProbarConexionInternet()
        {
            Resultado resultado = new Resultado();
            Uri url = new Uri("http://www.google.com");

            WebRequest webRequest;
            WebResponse webResponse;
            webRequest = WebRequest.Create(url);
            try
            {
                webResponse = webRequest.GetResponse();
                webResponse.Close();
                webRequest = null;
                resultado.Estado = true;
            }
            catch (Exception)
            {
                resultado.Estado = false;
                resultado.Mensaje = "No se ha podido establecer una conexión a internet...";
            }
            return resultado;
        }

        public static Resultado ProbarCertificado(String certificado, String clave)
        {
            Resultado resultado = new Resultado();
            try
            {
                var certificate = LoadCertificate(certificado, clave, out PrivateKey privateKey, out Provider provider);
                //Si encontramos el certificado...
                if (certificate != null)
                {
                    resultado.Estado = true;
                    resultado = ValidezCertificado(certificado, clave);
                    resultado.Mensaje = "Certificado Encontrado.\n" + resultado.Mensaje;
                }
                else
                {
                    resultado.Estado = false;
                    resultado.Mensaje = "No se encontro el certificado.";
                }
                privateKey = null;
                provider = null;
                certificate = null;
            }
            catch (Exception ex)
            {
                resultado.Estado = false;
                resultado.Mensaje = ex.Message;
            }
            return resultado;
        }

        public static Resultado ValidezCertificado(String certificado, String clave)
        {
            Resultado resultado = new Resultado();
            try
            {
                using (System.Security.Cryptography.X509Certificates.X509Certificate2 certificate =
                    new System.Security.Cryptography.X509Certificates.X509Certificate2(certificado, clave))
                {
                    if (certificate != null)
                    {

                        DateTime dateInit = Convert.ToDateTime(certificate.GetEffectiveDateString());
                        DateTime dateExpiration = Convert.ToDateTime(certificate.GetExpirationDateString());
                        if ((DateTime.Compare(DateTime.Now, dateInit) >= 0) && (DateTime.Compare(DateTime.Now, dateExpiration) <= 0))
                        {
                            resultado.Estado = true;
                            resultado.Mensaje = "Fechas del certificado son válidas:\t desde: " +
                                dateInit.ToString() + " hasta: " + dateExpiration.ToString();
                        }
                        else
                        {
                            resultado.Estado = false;
                            resultado.Mensaje = "Certificado caducado: Fecha Emisión: " +
                                dateInit.ToShortDateString() + "\nFecha de Caducidad: " + dateExpiration.ToShortDateString();
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                resultado.Estado = false;
                resultado.Mensaje = ex.Message;
            }
            return resultado;
        }

        private static X509Certificate LoadCertificate(string path, string password, out PrivateKey privateKey, out Provider provider)
        {
            X509Certificate certificate = null;
            provider = null;
            privateKey = null;

            //Cargar certificado de fichero PFX (p12) 
            KeyStore ks = KeyStore.getInstance("PKCS12");
            ks.load(new FileInputStream(path), password.ToCharArray());
            IPKStoreManager storeManager = new KSStore(ks, new PassStoreKS(password));
            List certificates = storeManager.getSignCertificates();

            //Si encontramos el certificado...  
            if (certificates.size() >= 1)
            {
                // Traer el certificado validado por el Banco Central
                certificate = (X509Certificate)certificates.get(1);
                // Obtención de la clave privada asociada al certificado  
                privateKey = storeManager.getPrivateKey(certificate);
                // Obtención del provider encargado de las labores criptográficas  
                provider = storeManager.getProvider(certificate);
            }

            return certificate;

        }

        public static Resultado Firmar(DocumentoElectronico documento, String certificado, String clave, Directorio directorio, string table)
        {
            Resultado resultado = new Resultado();
            Resultado pathSinFirma = directorio.Path(EstadoDocumento.SinFirma);
            Resultado pathFirmado = directorio.Path(EstadoDocumento.Firmado);
            Resultado validezCertificado = ValidezCertificado(certificado, clave);
            if (pathSinFirma.Estado && pathFirmado.Estado && validezCertificado.Estado)
            {
                try
                {
                    PrivateKey privateKey = null;
                    Provider provider = null;

                    //Traer certificado validado por el Banco Central
                    X509Certificate certificate = LoadCertificate(certificado, clave, out privateKey, out provider);

                    if (certificate != null)
                    {
                        TrustFactory.instance = TrustExtendFactory.newInstance();
                        TrustFactory.truster = MyPropsTruster.getInstance();
                        PoliciesManager.POLICY_SIGN = new es.mityc.javasign.xml.xades.policy.facturae.Facturae31Manager();

                        com.sun.org.apache.xerces.@internal.jaxp.SAXParserFactoryImpl s = 
                            new com.sun.org.apache.xerces.@internal.jaxp.SAXParserFactoryImpl();

                        PoliciesManager.POLICY_VALIDATION = 
                            new es.mityc.javasign.xml.xades.policy.facturae.Facturae31Manager();

                        // Sacado del SRI
                        DataToSign datosAFirmar = new DataToSign();
                        datosAFirmar.setXadesFormat(EnumFormatoFirma.XAdES_BES);
                        datosAFirmar.setEsquema(XAdESSchemas.XAdES_132);
                        datosAFirmar.setXMLEncoding("UTF-8");
                        datosAFirmar.setEnveloped(true);
                        datosAFirmar.addObject(new ObjectToSign(
                            (AbstractObjectToSign)new InternObjectToSign("comprobante"), "contenido comprobante", null, "text/xml", null));
                        datosAFirmar.setParentSignNode("comprobante");

                        Document doc = LoadXML(pathSinFirma.Mensaje + documento.Nombre + ".xml");
                        datosAFirmar.setDocument(doc);

                        // SRI
                        FirmaXML firma = new FirmaXML();
                        Object[] res = firma.signFile(certificate, datosAFirmar, privateKey, provider);

                        FileOutputStream file = new FileOutputStream(pathFirmado.Mensaje + documento.Nombre + ".xml");

                        UtilidadTratarNodo.saveDocumentToOutputStream((Document)res[0], file, true);

                        // Guardamos la firma a un fichero en el home del usuario
                        FileOutputStream outputStream = new FileOutputStream(pathFirmado.Mensaje + documento.Nombre + ".xml");
                        UtilidadTratarNodo.saveDocumentToOutputStream((Document)res[0], outputStream, true);
                        outputStream.close();
                        documento.Estado = EstadoDocumento.Firmado;
                        resultado.Estado = true;
                        System.IO.File.Delete(pathSinFirma.Mensaje + documento.Nombre + ".xml");

                        file.flush();
                        file.close();

                        //resultado = Consultas.UpdateEstadoFactura(documento.Id, table, "PPR");
                        resultado = Consultas.UpdateData(Queries.UpdateEstadoFactura(), table, "FIR", documento.Id);
                    }
                }
                catch (Exception exc)
                {
                    documento.Estado = EstadoDocumento.SinFirma;
                    resultado.Estado = false;
                    resultado.Mensaje = exc.Message;
                }
            }
            else
            {
                resultado.Estado = false;
                if (!pathSinFirma.Estado)
                    resultado.Mensaje = "No se ha conseguido cargar el directorio Inicial:\n" + pathSinFirma.Mensaje;
                if (!pathFirmado.Estado)
                    resultado.Mensaje += "No se ha conseguido cargar el directorio de Documentos Firmados:\n" + pathFirmado.Mensaje;
                if (!validezCertificado.Estado)
                    resultado.Mensaje += "Problemas con el Certificado Digital: " + validezCertificado.Mensaje;
            }
            return resultado;
        }

        private static Document LoadXML(string SinFirma)
        {
            DocumentBuilderFactory dbf = DocumentBuilderFactory.newInstance();
            dbf.setNamespaceAware(true);
            return dbf.newDocumentBuilder().parse(new BufferedInputStream(new FileInputStream(SinFirma)));
        }

        //Enviar el Documento al SRI para que sa recibido o devuelto
        public static Resultado ValidarSRI(DocumentoElectronico documento, Directorio directorio, string table)
        {
            Resultado resultado = new Resultado();
            Resultado pathFirmados = directorio.Path(EstadoDocumento.Firmado);
            Resultado pathRecibidos = directorio.Path(EstadoDocumento.Recibido);
            Resultado pathDevueltos = directorio.Path(EstadoDocumento.Devuelto);
            if (pathFirmados.Estado && pathRecibidos.Estado & pathDevueltos.Estado)
            {
                byte[] signedXml = System.IO.File.ReadAllBytes(pathFirmados.Mensaje + documento.Nombre + ".xml");
                int intento = 1;
                if (signedXml[0].ToString() != "60")
                    signedXml = signedXml.Skip(3).ToArray();
                Found:
                try
                {
                    using (RecepcionComprobantesService1 recepComService = new RecepcionComprobantesService1())
                    {
                        // Cambiar url dependiendo el ambiente
                        recepComService.Url = Properties.Settings.Default.SiCtert_FacturaElectrónica_wsRecepcionSRI_RecepcionComprobantesOfflineService;
                        wsRecepcionSRI.respuestaSolicitud response = new wsRecepcionSRI.respuestaSolicitud();
                        //respuestaSolicitud  
                        response = recepComService.validarComprobante(signedXml);
                        documento.SoapRecepción = recepComService.XmlResponse;
                        var resp = documento.Recepción;

                        if (resp.estado.ToUpper() == "RECIBIDA")
                        {
                            //resultado = Consultas.UpdateEstadoFactura(documento.Id, table, "ENV");
                            resultado = Consultas.UpdateData(Queries.UpdateEstadoFactura(), table, "ENV", documento.Id);
                            documento.Estado = EstadoDocumento.Recibido;
                            resultado.Estado = true;
                            
                            System.IO.File.Delete(pathRecibidos.Mensaje + documento.Nombre + ".xml");
                            System.IO.File.Copy(pathFirmados.Mensaje + documento.Nombre + ".xml", 
                                pathRecibidos.Mensaje + documento.Nombre + ".xml");
                        }
                        else
                        {
                            //Si tiene algún problema
                            //resultado = Consultas.UpdateEstadoFactura(documento.Id, table, "NEV");
                            resultado = Consultas.UpdateData(Queries.UpdateEstadoFactura(), table, "NEV", documento.Id);
                            documento.Estado = EstadoDocumento.Devuelto;
                            resultado.Estado = false;
                            if (documento.Mensaje == null) documento.Mensaje = "";
                            documento.Mensaje = resultado.Mensaje;

                            System.IO.File.Delete(pathDevueltos.Mensaje + documento.Nombre + ".xml");
                            System.IO.File.Move(pathFirmados.Mensaje + documento.Nombre + ".xml",
                                pathDevueltos.Mensaje + documento.Nombre + ".xml");
                            resultado.Mensaje = resp.comprobantes.comprobante.mensajes.mensaje.mensaje + " => " +
                                resp.comprobantes.comprobante.mensajes.mensaje.informacionAdicional;
                        }
                        
                    }
                }
                catch (WebException ew)
                {
                    resultado.Estado = false;
                    resultado.Mensaje += (ew.Message + "\n");
                    if (intento < 3)
                    {
                        intento++;
                        Thread.Sleep(2000);
                        goto Found;
                    }
                }
                catch (Exception ex)
                {
                    resultado.Estado = false;
                    resultado.Mensaje += (ex.Message + "\n");
                    if (intento < 3)
                    {
                        intento++;
                        Thread.Sleep(6000);
                        goto Found;
                    }
                }
            }
            else
            {
                resultado.Mensaje = "Error: ";
                resultado.Estado = false;

                if (!pathFirmados.Estado)
                    resultado.Mensaje += "\nNo se ha encontrado el directorio de los documentos Firmados.";
                if (!pathRecibidos.Estado)
                    resultado.Mensaje += "\nNo se ha encontrado el directorio de los documentos recibidos.";
                if (!pathDevueltos.Estado)
                    resultado.Mensaje += "\nNo se ha encontrado el directorio de los documentos no recibidos.";
            }
            return resultado;
        }

        public static Resultado ConsultarSRI(DocumentoElectronico documento, Directorio directorio, string table)
        {
            int intento = 1;
            string mXML = "";
            intento = 1;
            Resultado resultado = new Resultado();
            Resultado pathFirmados = directorio.Path(EstadoDocumento.Firmado);
            Resultado pathRecibido = directorio.Path(EstadoDocumento.Recibido);
            Resultado pathAutorizado = directorio.Path(EstadoDocumento.Autorizado);
            Resultado pathRechazado = directorio.Path(EstadoDocumento.Rechazado);
        Consultar:
            if (pathRecibido.Estado && pathAutorizado.Estado && pathRechazado.Estado)
            {
                //  string dxml = System.IO.File.ReadAllText(doc.Mensaje + doc.Nombre + ".xml");
                //string nError = "";
                //string mError = "";
                try
                {
                    using (AutorizacionComprobantesService1 autCompService = new AutorizacionComprobantesService1())
                    {
                        string estadoAutorizado = String.Empty;
                        string fechaAutorizacion = String.Empty;

                        wsAutorizacionSRI.respuestaComprobante response = new wsAutorizacionSRI.respuestaComprobante();
                        //ac2.Url = Program.getServiciosSRI.FirstOrDefault(x => x.Ambiente == doc.Ambiente).Autorización;
                        autCompService.Url = Properties.Settings.Default.SiCtert_FacturaElectrónica_wsAutorizacionSRI_AutorizacionComprobantesOfflineService;
                        response = autCompService.autorizacionComprobante(documento.ClaveAcceso);
                        mXML = autCompService.XmlResponse;
                        documento.SoapValidar = mXML;
                        var resp = documento.Autorización;


                        //XmlNodeList xnList = xml.SelectNodes("/data/Table");

                        try
                        {
                            estadoAutorizado = resp.autorizaciones[0].estado;
                            fechaAutorizacion = resp.autorizaciones[0].fechaAutorizacion;
                        }
                        catch { resultado.Mensaje = "Ok"; }

                        if (resp.autorizaciones.FirstOrDefault().estado.Trim(' ') == "AUTORIZADO")
                        {
                            //resultado = Consultas.UpdateEstadoFactura(documento.Id, table, "AUT");
                            resultado = Consultas.UpdateData(Queries.UpdateEstadoFactura(), table, "AUT", documento.Id);
                            //resultado.Estado = true;
                            // Ingresar factura firmada en el formato del SRI cuando es autorizado
                            if (!string.IsNullOrEmpty(estadoAutorizado) && !string.IsNullOrEmpty(documento.ClaveAcceso) && !string.IsNullOrEmpty(fechaAutorizacion))
                            {
                                // Crear el xml de autorización a partir del xml de firma.
                                XmlDocument doc = new XmlDocument();
                                XmlDeclaration xmlDeclaration = doc.CreateXmlDeclaration("1.0", "UTF-8", "yes");
                                XmlNode root = doc.DocumentElement;
                                doc.InsertBefore(xmlDeclaration, root);
                                XmlElement raizAutorizacion = doc.CreateElement("autorizacion");
                                doc.AppendChild(raizAutorizacion);
                                XmlElement estado = doc.CreateElement("estado");
                                estado.AppendChild(doc.CreateTextNode(estadoAutorizado));
                                raizAutorizacion.AppendChild(estado);

                                XmlElement numeroAutorizacion = doc.CreateElement("numeroAutorizacion");
                                numeroAutorizacion.AppendChild(doc.CreateTextNode(documento.ClaveAcceso));
                                raizAutorizacion.AppendChild(numeroAutorizacion);

                                XmlElement fechaAutorizacionXml = doc.CreateElement("fechaAutorizacion");
                                fechaAutorizacionXml.SetAttribute("class", "fechaAutorizacion");
                                fechaAutorizacionXml.AppendChild(doc.CreateTextNode(fechaAutorizacion));
                                raizAutorizacion.AppendChild(fechaAutorizacionXml);
                                string xmlText = System.IO.File.ReadAllText(pathFirmados.Mensaje + documento.Nombre + ".xml");
                                var cdata = new XmlDocument();
                                cdata.LoadXml(xmlText);

                                XmlElement comprobante = doc.CreateElement("comprobante");
                                comprobante.AppendChild(doc.CreateCDataSection(xmlText.ToString()));
                                raizAutorizacion.AppendChild(comprobante);

                                XmlElement mensaje = doc.CreateElement("mensajes");
                                raizAutorizacion.AppendChild(mensaje);
                                doc.Save(pathAutorizado.Mensaje + documento.Nombre + ".xml");
                                System.IO.File.Delete(pathRecibido.Mensaje + documento.Nombre + ".xml");

                                documento.Estado = EstadoDocumento.Autorizado;
                                resultado.Estado = true;
                                resultado.Mensaje = $"{estadoAutorizado} en {fechaAutorizacion}";
                            }
                        }
                        else
                        {
                            documento.Estado = EstadoDocumento.Rechazado;
                            resultado.Estado = false;
                            System.IO.File.WriteAllText(pathRechazado.Mensaje + documento.Nombre + ".xml", documento.SoapValidar);
                            //resultado = Consultas.UpdateEstadoFactura(documento.Id, table, "NAT");
                            resultado = Consultas.UpdateData(Queries.UpdateEstadoFactura(), table, "NAT", documento.Id);
                        }
                    }

                }
                catch (WebException we)
                {
                    resultado.Estado = false;
                    resultado.Mensaje += we.Message + "\n";
                    if (intento < 3)
                    {
                        intento++;
                        Thread.Sleep(2000);
                        goto Consultar;
                    }
                }
                catch (Exception ex)
                {
                    resultado.Estado = false;
                    resultado.Mensaje += ex.Message + "\n";
                }
            }
            return resultado;
        }
    
    }
}
