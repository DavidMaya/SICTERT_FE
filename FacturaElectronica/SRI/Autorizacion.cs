using System;
using System.CodeDom.Compiler;
using System.ComponentModel;
using System.Diagnostics;
using System.Xml.Serialization;

namespace FacturaElectronica.SRI
{
    [GeneratedCodeAttribute("xsd", "4.0.30319.33440")]
    [SerializableAttribute]
    [DebuggerStepThroughAttribute()]
    [DesignerCategoryAttribute("code")]
    [XmlTypeAttribute(AnonymousType = true)]
    public partial class autorizacionComprobante
    {
        public factura factura { get; set; }
        //public comprobanteRetencion comprobanteRetencion { get; set; }
        //public notaCredito notaCredito { get; set; }
    }
    [Serializable]
    [XmlRoot("RespuestaAutorizacionComprobante")]
    public partial class RespuestaAutorizacion
    {
        public string claveAccesoConsultada { get; set; }
        public string numeroComprobantes { get; set; }
        public autorizacion[] autorizaciones { get; set; }

    }

    [GeneratedCodeAttribute("xsd", "4.0.30319.33440")]
    [SerializableAttribute]
    [DebuggerStepThroughAttribute()]
    [DesignerCategoryAttribute("code")]
    [XmlTypeAttribute(AnonymousType = true)]
    [System.Xml.Serialization.XmlRootAttribute(Namespace = "", IsNullable = false)]
    public partial class autorizacion
    {
        public string estado { get; set; }
        public string numeroAutorizacion { get; set; }
        public string fechaAutorizacion { get; set; }
        public string ambiente { get; set; }
        public autorizacionComprobante comprobante { get; set; }
        public autorizacionMensajes mensajes { get; set; }

    }

    [GeneratedCodeAttribute("xsd", "4.0.30319.33440")]
    [SerializableAttribute]
    [DebuggerStepThroughAttribute()]
    [DesignerCategoryAttribute("code")]
    [XmlTypeAttribute(AnonymousType = true)]
    public partial class autorizacionMensajes
    {
        
        public autorizacionMensaje mensaje { get; set; }

    }

    [GeneratedCodeAttribute("xsd", "4.0.30319.33440")]
    [SerializableAttribute]
    [DebuggerStepThroughAttribute()]
    [DesignerCategoryAttribute("code")]
    [XmlTypeAttribute(AnonymousType = true)]
    public partial class autorizacionMensaje
    {
        public string identificador { get; set; }
        public string mensaje { get; set; }
        public string tipo { get; set; }

    }
}
