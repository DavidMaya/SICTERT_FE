using FacturaElectronica.SRI;
using FacturaElectronica.Tools;
using System;
using System.IO;
using System.Linq;
using System.Xml.Serialization;

namespace FacturaElectronica.Documento
{
    [Serializable]
    public class DocumentoElectronico
    {
        public long Id { get; set; }
        public string Table { get; set; }
        public TipoDocumento Tipo { get; set; }
        public EstadoDocumento Estado { get; set; }
        public Boolean EnviadoMail { get; set; }
        public string xml { get; set; }
        public string SoapRecepción { get; set; }
        public string SoapValidar { get; set; }
        [XmlIgnore]
        public string Nombre
        {
            get
            {
                //return Tipo.ToString() + Id.ToString().PadLeft(5, '0');
                return $"{Table}_{Id.ToString().PadLeft(15, '0')}";
            }
        }
        [XmlIgnore]
        public string ClaveAcceso { get; set; }

        [XmlIgnore]
        public string SecuencialDocumento
        {
            get
            {
                string r = "";
                switch (Tipo)
                {
                    case TipoDocumento.Factura:
                        r = this.Tipo.ToString().ToUpper() + ": " +
                            this.Factura.infoTributaria.estab + "-" +
                            this.Factura.infoTributaria.ptoEmi + "-" +
                            this.Factura.infoTributaria.secuencial;
                        break;
                    //case TipoDocumento.NotaCredito:
                    //    r = this.Tipo.ToString().ToUpper() + ": " +
                    //       this.NotaCredito.infoTributaria.estab + "-" +
                    //       this.NotaCredito.infoTributaria.ptoEmi + "-" +
                    //       this.NotaCredito.infoTributaria.secuencial;
                    //    break;
                    //case TipoDocumento.Retencion:
                    //    r = this.Tipo.ToString().ToUpper() + ": " +
                    //        this.Retención.infoTributaria.estab + "-" +
                    //        this.Retención.infoTributaria.ptoEmi + "-" +
                    //        this.Retención.infoTributaria.secuencial;
                    //    break;
                    //case TipoDocumento.GuiaRemision:
                    //    break;
                }
                return r;
            }
        }
        [XmlIgnore]
        public string PersonaRecibe
        {
            get
            {
                string r = "";
                switch (Tipo)
                {
                    case TipoDocumento.Factura:
                        r = this.Factura.infoFactura.razonSocialComprador.ToUpper();
                        break;
                    //case TipoDocumento.NotaCredito:
                    //    r = this.NotaCredito.infoNotaCredito.razonSocialComprador.ToUpper();
                    //    break;
                    //case TipoDocumento.Retencion:
                    //    r = this.Retención.infoCompRetencion.razonSocialSujetoRetenido.ToUpper();
                    //    break;
                    //case TipoDocumento.GuiaRemision:
                    //    break;
                }
                return r;
            }
        }
        [XmlIgnore]
        public string mailRecibe
        {
            get
            {
                string r = "";
                switch (Tipo)
                {
                    case TipoDocumento.Factura:
                        r = this.Factura.infoAdicional.FirstOrDefault(x => x.nombre.ToUpper().Contains("EMAIL") == true).Value;
                        break;
                    //case TipoDocumento.NotaCredito:
                    //    r = this.NotaCredito.infoAdicional.FirstOrDefault(x => x.nombre.ToUpper().Contains("EMAIL") == true).Value;
                    //    break;
                    //case TipoDocumento.Retencion:
                    //    r = this.Retención.infoAdicional.FirstOrDefault(x => x.nombre.ToUpper().Contains("EMAIL") == true).Value;
                    //    break;
                    //case TipoDocumento.GuiaRemision:
                    //    break;
                }
                return r;
            }
        }

        public AmbienteActivo Ambiente { get; set; }

        public String Mensaje { get; set; }
        //public Directorio DirecciónTrabajo
        //{
        //    get
        //    {
        //        Directorio r = new Directorio();
        //        r.SinFirma = Dirección.SinFirma + ("\\" + Tipo.ToString() + Id.ToString().PadLeft(5, '0') + ".xml");
        //        r.Firmado = Dirección.Firmado + ("\\" + Tipo.ToString() + Id.ToString().PadLeft(5, '0') + ".xml");
        //        r.Válido = Dirección.Válido + ("\\" + Tipo.ToString() + Id.ToString().PadLeft(5, '0') + ".pdf");
        //        return r;
        //    }
        //}
        [XmlIgnore]
        public Stream RespuestaValidar
        {
            get
            {
                MemoryStream stream = new MemoryStream();
                StreamWriter writer = new StreamWriter(stream);
                writer.Write(SoapValidar);
                writer.Flush();
                stream.Position = 0;
                return stream;
            }
        }
        [XmlIgnore]
        public RespuestaRecepcion Recepción
        {
            get
            {
                RespuestaRecepcion r = null;
                string cx = "<RespuestaRecepcionComprobante>";
                string mXML = SoapRecepción;
                mXML = mXML.Substring(mXML.IndexOf(cx));
                mXML = mXML.Substring(0, mXML.IndexOf(cx.Replace("<", "</")) + cx.Length + 1);


                try
                {

                    r = XmlTools.Deserialize<RespuestaRecepcion>(mXML);
                }
                catch { r = null; }
                return r;
            }
        }

        [XmlIgnore]
        public RespuestaAutorizacion Autorización
        {
            get
            {
                RespuestaAutorizacion r = null;
                string cR = "<RespuestaAutorizacionComprobante>";
                string Respuesta = SoapValidar;
                Respuesta = Respuesta.Substring(Respuesta.IndexOf(cR));
                Respuesta = Respuesta.Substring(0, Respuesta.IndexOf(cR.Replace("<", "</")) + cR.Length + 1);
                Respuesta = Respuesta.Replace("<?xml version=\"1.0\" encoding=\"UTF-8\"?>", "<!-- <?xml version=\"1.0\" encoding=\"UTF-8\"?> -->");
                try
                {
                    r = XmlTools.Deserialize<RespuestaAutorizacion>(Respuesta);
                }
                catch { r = null; }
                return r;
            }
        }

        public factura Factura
        {
            get
            {
                factura r = null;
                if (Tipo == TipoDocumento.Factura)
                {
                    r = XmlTools.Deserialize<factura>(xml);
                }
                return r;
            }
        }

        //public comprobanteRetencion Retención
        //{
        //    get
        //    {
        //        comprobanteRetencion r = null;
        //        if (Tipo == TipoDocumento.Retencion)
        //        {
        //            try
        //            {
        //                r = Deserializar.Valor<comprobanteRetencion>(xml);
        //            }
        //            catch { r = null; }
        //        }
        //        return r;
        //    }
        //}

        //public notaCredito NotaCredito
        //{
        //    get
        //    {
        //        notaCredito r = null;
        //        if (Tipo == TipoDocumento.NotaCredito)
        //        {
        //            r = Deserializar.Valor<notaCredito>(xml);
        //        }
        //        return r;
        //    }
        //}

        public string certificado { get; set; }
        public string clave { get; set; }

    }
}
