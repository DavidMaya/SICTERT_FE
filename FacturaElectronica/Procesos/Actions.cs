using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
using System.Net.Mail;
using System.IO;
using static System.Net.Mime.MediaTypeNames;
using System.Net;
using System.Threading;
using sviudes.blogspot.com;
using FacturaElectronica.Documento;
using FacturaElectronica.Tools;

namespace FacturaElectronica.Procesos
{
    public static class Actions
    {
        public static Resultado ProbarCertificado(String certificado, String clave)
        {
            Resultado resultado = new Resultado();
            try
            {
                PrivateKey privateKey;
                Provider provider;
                var certificate = LoadCertificate(certificado, clave, out privateKey, out provider);
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
                System.Security.Cryptography.X509Certificates.X509Certificate2 m_cer =
                    new System.Security.Cryptography.X509Certificates.X509Certificate2(certificado, clave);


                if (m_cer != null)
                {

                    DateTime fi = Convert.ToDateTime(m_cer.GetEffectiveDateString());
                    DateTime ff = Convert.ToDateTime(m_cer.GetExpirationDateString());
                    if ((DateTime.Compare(DateTime.Now, fi) >= 0) && (DateTime.Compare(DateTime.Now, ff) <= 0))
                    {
                        resultado.Estado = true;
                        resultado.Mensaje = "Fechas del certificado son válidas:\t desde: " + fi.ToString() + " hasta: " + ff.ToString();
                    }
                    else
                    {
                        resultado.Estado = false;
                        resultado.Mensaje = "Certificado caducado: Fecha Emisión: " + fi.ToShortDateString() +
                                    "\nFecha de Caducidad: " + ff.ToShortDateString();
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
            // David: Comentado para probar nuevos cambios

            X509Certificate certificate = null;
            provider = null;
            privateKey = null;

            ////Cargar certificado de fichero PFX
            //KeyStore ks = KeyStore.getInstance("PKCS12");
            //ks.load(new BufferedInputStream(new FileInputStream(path)), password.ToCharArray());
            //IPKStoreManager storeManager = new KSStore(ks, new PassStoreKS(password));
            //List certificates = storeManager.getSignCertificates();

            ////Si encontramos el certificado...
            //if (certificates != null)
            //{
            //    certificate = (X509Certificate)certificates.get(1);

            //    // Obtención de la clave privada asociada al certificado
            //    privateKey = storeManager.getPrivateKey(certificate);

            //    // Obtención del provider encargado de las labores criptográficas
            //    provider = storeManager.getProvider(certificate);
            //}

            //Cargar certificado de fichero PFX  
            KeyStore ks = KeyStore.getInstance("PKCS12");
            ks.load(new FileInputStream(path), password.ToCharArray());
            IPKStoreManager storeManager = new KSStore(ks, new PassStoreKS(password));
            List certificates = storeManager.getSignCertificates();

            //Si encontramos el certificado...  
            if (certificates.size() >= 1)
            {
                certificate = (X509Certificate)certificates.get(1);

                // Obtención de la clave privada asociada al certificado  
                privateKey = storeManager.getPrivateKey(certificate);

                // Obtención del provider encargado de las labores criptográficas  
                provider = storeManager.getProvider(certificate);
            }

            return certificate;

        }

        public static Resultado Firmar(DocumentoElectronico Documento, String certificado, String clave, Directorio directorio)
        {
            Resultado resultado = new Resultado();
            Resultado pathOrigen = directorio.Path(EstadoDocumento.SinFirma);
            Resultado pathDestino = directorio.Path(EstadoDocumento.Firmado);
            Resultado validezCertificado = ValidezCertificado(certificado, clave);
            if (pathOrigen.Estado && pathDestino.Estado && validezCertificado.Estado)
            {
                try
                {
                    PrivateKey privateKey = null;
                    Provider provider = null;

                    //Sacado del SRI.
                    X509Certificate certificate = LoadCertificate(certificado, clave, out privateKey, out provider);

                    if (certificate != null)
                    {
                        TrustFactory.instance = TrustExtendFactory.newInstance();
                        TrustFactory.truster = MyPropsTruster.getInstance();
                        PoliciesManager.POLICY_SIGN = new es.mityc.javasign.xml.xades.policy.facturae.Facturae31Manager();

                        com.sun.org.apache.xerces.@internal.jaxp.SAXParserFactoryImpl s = new com.sun.org.apache.xerces.@internal.jaxp.SAXParserFactoryImpl();

                        PoliciesManager.POLICY_VALIDATION = new es.mityc.javasign.xml.xades.policy.facturae.Facturae31Manager();

                        // Sacado del SRI
                        DataToSign datosAFirmar = new DataToSign();
                        datosAFirmar.setXadesFormat(EnumFormatoFirma.XAdES_BES);
                        datosAFirmar.setEsquema(XAdESSchemas.XAdES_132);
                        datosAFirmar.setXMLEncoding("UTF-8");
                        datosAFirmar.setEnveloped(true);
                        datosAFirmar.addObject(new ObjectToSign(
                            (AbstractObjectToSign)new InternObjectToSign("comprobante"), "contenido comprobante", null, "text/xml", null));
                        datosAFirmar.setParentSignNode("comprobante");

                        //System.IO.File.WriteAllText(pathOrigen.Mensaje + Documento.Nombre + ".xml", Documento.xml);
                        Document docToSign = LoadXML(pathOrigen.Mensaje + Documento.Nombre + ".xml");

                        Document doc = LoadXML(pathOrigen.Mensaje + Documento.Nombre + ".xml");
                        datosAFirmar.setDocument(doc);

                        // SRI:
                        FirmaXML firma = new FirmaXML();
                        Object[] res = firma.signFile(certificate, datosAFirmar, privateKey, provider);

                        FileOutputStream file = new FileOutputStream(pathDestino.Mensaje + Documento.Nombre + ".xml");

                        UtilidadTratarNodo.saveDocumentToOutputStream((Document)res[0], file, true);

                        // Guardamos la firma a un fichero en el home del usuario
                        FileOutputStream outputStream = new FileOutputStream(pathDestino.Mensaje + Documento.Nombre + ".xml");
                        UtilidadTratarNodo.saveDocumentToOutputStream((Document)res[0], outputStream, true);
                        outputStream.close();
                        Documento.Estado = EstadoDocumento.Firmado;
                        resultado.Estado = true;
                        System.IO.File.Delete(pathOrigen.Mensaje + Documento.Nombre + ".xml");

                        file.flush();
                        file.close();

                    }
                }
                catch (Exception exc)
                {
                    Documento.Estado = EstadoDocumento.SinFirma;
                    resultado.Estado = false;
                    resultado.Mensaje = exc.Message;
                }
            }
            else
            {
                resultado.Estado = false;
                if (!pathOrigen.Estado)
                    resultado.Mensaje = "No se ha conseguido cargar el directorio Inicial:\n" + pathOrigen.Mensaje;
                if (!pathDestino.Estado)
                    resultado.Mensaje += "No se ha conseguido cargar el directorio de Documentos Firmados:\n" + pathDestino.Mensaje;
                if (!validezCertificado.Estado)
                    resultado.Mensaje += "Problemas con el Certificado Digital: " + validezCertificado.Mensaje;
                pathOrigen = null;
                pathDestino = null;
                validezCertificado = null;
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
    }
}
