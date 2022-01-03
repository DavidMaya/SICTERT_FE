using System;
using System.CodeDom.Compiler;
using System.ComponentModel;
using System.Diagnostics;
using System.Xml.Serialization;

namespace FacturaElectronica.SRI
{
    [GeneratedCodeAttribute("xsd", "4.0.30319.33440")]
    [SerializableAttribute()]
    [DebuggerStepThroughAttribute()]
    [DesignerCategoryAttribute("code")]
    [XmlRoot("validarComprobanteResponse")]

    public partial class validarComprobante
    {
        public RespuestaRecepcion RespuestaRecepcionComprobante { get; set; }
    }

    /// <comentarios/>
    [GeneratedCodeAttribute("xsd", "4.0.30319.33440")]
    [SerializableAttribute()]
    [DebuggerStepThroughAttribute()]
    [DesignerCategoryAttribute("code")]
    [XmlTypeAttribute(AnonymousType = true)]
    [XmlRoot("RespuestaRecepcionComprobante")]
    public partial class RespuestaRecepcion
    {
        public string estado { get; set; }
        public RecepcionComprobantes comprobantes { get; set; }
    }

    /// <comentarios/>
    [GeneratedCodeAttribute("xsd", "4.0.30319.33440")]
    [SerializableAttribute()]
    [DebuggerStepThroughAttribute()]
    [DesignerCategoryAttribute("code")]
    [XmlTypeAttribute(AnonymousType = true)]
    [XmlRoot("comprobantes")]
    public partial class RecepcionComprobantes
    {
        public RecepcionComprobante comprobante { get; set; }
    }

    /// <comentarios/>
    [GeneratedCodeAttribute("xsd", "4.0.30319.33440")]
    [SerializableAttribute()]
    [DebuggerStepThroughAttribute()]
    [DesignerCategoryAttribute("code")]
    [XmlTypeAttribute(AnonymousType = true)]
    [XmlRoot("comprobante")]
    public partial class RecepcionComprobante
    {
        public string claveAcceso { get; set; }
        public ComprobanteMensajes mensajes { get; set; }
    }

    /// <comentarios/>
    [GeneratedCodeAttribute("xsd", "4.0.30319.33440")]
    [SerializableAttribute()]
    [DebuggerStepThroughAttribute()]
    [DesignerCategoryAttribute("code")]
    [XmlTypeAttribute(AnonymousType = true)]
    [XmlRoot("mensajes")]
    public partial class ComprobanteMensajes
    {
        /// <comentarios/>
        public ComprobanteMensaje mensaje { get; set; }
    }

    /// <comentarios/>
    [GeneratedCodeAttribute("xsd", "4.0.30319.33440")]
    [SerializableAttribute()]
    [DebuggerStepThroughAttribute()]
    [DesignerCategoryAttribute("code")]
    [XmlTypeAttribute(AnonymousType = true)]
    [XmlRoot("mensaje")]
    public partial class ComprobanteMensaje
    {

        public sbyte identificador { get; set; }
        public string mensaje { get; set; }
        public string informacionAdicional { get; set; }
        public string tipo { get; set; }
    }
}
