using System;
using System.CodeDom.Compiler;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Xml.Serialization;

namespace FacturaElectronica.SRI
{
    [GeneratedCodeAttribute("xsd", "4.0.30319.33440")]
    [SerializableAttribute()]
    [DebuggerStepThroughAttribute()]
    [DesignerCategoryAttribute("code")]
    [XmlTypeAttribute(AnonymousType = true)]
    [XmlRootAttribute(Namespace = "", IsNullable = false)]
    public class notaCredito
    {
        public InfoTributaria infoTributaria { get; set; }
        public InfoNotaCredito infoNotaCredito { get; set; }

        [XmlArrayItemAttribute("detalle", IsNullable = false)]
        public List<Detalle> detalles { get; set; }
        public MaquinaFiscal maquinaFiscal { get; set; }
        [XmlArrayItemAttribute("campoAdicional", IsNullable = false)]
        public List<InfoAdicional> infoAdicional { get; set; }

        [XmlAttributeAttribute("id")]
        public string id { get; set; }

        [XmlAttributeAttribute("version")]
        public string version { get; set; }

        public notaCredito()
        {
            //version = "1.1.0"; // Versión de Factura
            //id = "comprobante";
            infoTributaria = new InfoTributaria();
            infoNotaCredito = new InfoNotaCredito();
            infoNotaCredito.compensaciones = new List<Compensacion>();
            infoNotaCredito.totalConImpuestos = new List<TotalImpuesto>();
            detalles = new List<Detalle>();
            //maquinaFiscal = new MaquinaFiscal();
            infoAdicional = new List<InfoAdicional>();
        }
    }

    [GeneratedCodeAttribute("xsd", "4.0.30319.33440")]
    [SerializableAttribute()]
    [DebuggerStepThroughAttribute()]
    [DesignerCategoryAttribute("code")]
    [XmlTypeAttribute(AnonymousType = true)]
    public class InfoTributaria
    {
        public int ambiente { get; set; }
        public int tipoEmision { get; set; }
        public string razonSocial { get; set; }
        public string nombreComercial { get; set; }
        public string ruc { get; set; }
        public string claveAcceso { get; set; }
        public string codDoc { get; set; }
        public string estab { get; set; }
        public string ptoEmi { get; set; }
        public string secuencial { get; set; }
        public string dirMatriz { get; set; }
        public string regimenMicroempresas { get; set; }
        public string agenteRetencion { get; set; }
        public string contribuyenteRimpe { get; set; }
    }

    [GeneratedCodeAttribute("xsd", "4.0.30319.33440")]
    [SerializableAttribute()]
    [DebuggerStepThroughAttribute()]
    [DesignerCategoryAttribute("code")]
    [XmlTypeAttribute(AnonymousType = true)]
    public class InfoNotaCredito
    {
        public string fechaEmision { get; set; }
        public string dirEstablecimiento { get; set; }
        public string tipoIdentificacionComprador { get; set; }
        public string razonSocialComprador { get; set; }
        public string identificacionComprador { get; set; }
        public string contribuyenteEspecial { get; set; }
        public string obligadoContabilidad { get; set; }
        public string rise { get; set; }
        public string codDocModificado { get; set; }
        public string numDocModificado { get; set; }
        public string fechaEmisionDocSustento { get; set; }
        public string totalSinImpuestos { get; set; }
        [XmlArrayItemAttribute("compensacion", IsNullable = false)]
        public List<Compensacion> compensaciones { get; set; }
        public string valorModificacion { get; set; }
        public string moneda { get; set; }
        [XmlArrayItemAttribute("totalImpuesto", IsNullable = false)]
        public List<TotalImpuesto> totalConImpuestos { get; set; }
        public string motivo { get; set; }
    }

    [GeneratedCodeAttribute("xsd", "4.0.30319.33440")]
    [SerializableAttribute()]
    [DebuggerStepThroughAttribute()]
    [DesignerCategoryAttribute("code")]
    [XmlTypeAttribute(AnonymousType = true)]
    public partial class Compensacion
    {
        public string codigo { get; set; }
        public string tarifa { get; set; }
        public string valor { get; set; }
    }

    [GeneratedCodeAttribute("xsd", "4.0.30319.33440")]
    [SerializableAttribute()]
    [DebuggerStepThroughAttribute()]
    [DesignerCategoryAttribute("code")]
    [XmlTypeAttribute(AnonymousType = true)]
    public class TotalImpuesto
    {
        public string codigo { get; set; }
        public string codigoPorcentaje { get; set; }
        public string baseImponible { get; set; }
        public string valor { get; set; }
        public string valorDevolucionIva { get; set; }
    }

    [GeneratedCodeAttribute("xsd", "4.0.30319.33440")]
    [SerializableAttribute()]
    [DebuggerStepThroughAttribute()]
    [DesignerCategoryAttribute("code")]
    [XmlTypeAttribute(AnonymousType = true)]
    public class Detalle
    {
        public string codigoInterno { get; set; }
        public string codigoAdicional { get; set; }
        public string descripcion { get; set; }
        public string cantidad { get; set; }
        public string precioUnitario { get; set; }
        public string descuento { get; set; }
        public string precioTotalSinImpuesto { get; set; }
        [XmlArrayItem("detAdicional", IsNullable = false)]
        public List<DetAdicional> detallesAdicionales { get; set; }
        [XmlArrayItem("impuesto", IsNullable = false)]
        public List<Impuestos> impuestos { get; set; }
    }

    [GeneratedCodeAttribute("xsd", "4.0.30319.33440")]
    [SerializableAttribute()]
    [DebuggerStepThroughAttribute()]
    [DesignerCategoryAttribute("code")]
    [XmlTypeAttribute(AnonymousType = true)]
    public class DetAdicional
    {
        [XmlAttribute()]
        public string nombre { get; set; }
        [XmlAttribute()]
        public string valor { get; set; }
    }

    [GeneratedCodeAttribute("xsd", "4.0.30319.33440")]
    [SerializableAttribute()]
    [DebuggerStepThroughAttribute()]
    [DesignerCategoryAttribute("code")]
    [XmlTypeAttribute(AnonymousType = true)]
    public class Impuestos
    {
        public string codigo { get; set; }
        public string codigoPorcentaje { get; set; }
        public string tarifa { get; set; }
        public string baseImponible { get; set; }
        public string valor { get; set; }
    }

    [GeneratedCodeAttribute("xsd", "4.0.30319.33440")]
    [SerializableAttribute()]
    [DebuggerStepThroughAttribute()]
    [DesignerCategoryAttribute("code")]
    [XmlTypeAttribute(AnonymousType = true)]
    public class MaquinaFiscal
    {
        public string marca { get; set; }
        public string modelo { get; set; }
        public string serie { get; set; }
    }

    [GeneratedCodeAttribute("xsd", "4.0.30319.33440")]
    [SerializableAttribute()]
    [DebuggerStepThroughAttribute()]
    [DesignerCategoryAttribute("code")]
    [XmlTypeAttribute(AnonymousType = true)]
    public class InfoAdicional
    {
        [XmlAttributeAttribute()]
        public string nombre { get; set; }
        [XmlTextAttribute()]
        public string Value { get; set; }
    }
}
