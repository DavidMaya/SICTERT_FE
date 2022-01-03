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
    public partial class factura
    {
        public facturaInfoTributaria infoTributaria { get; set; }
        public facturaInfoFactura infoFactura { get; set; }

        [XmlArrayItemAttribute("detalle", IsNullable = false)]
        public List<facturaDetalle> detalles { get; set; }

        [XmlArrayItemAttribute("campoAdicional", IsNullable = false)]
        public List<facturaCampoAdicional> infoAdicional { get; set; }

        [XmlAttributeAttribute("id")]
        public string id { get; set; }

        [XmlAttributeAttribute("version")]
        public string version { get; set; }

        public factura()
        {
            version = "1.0.0";
            id = "comprobante";
            infoTributaria = new facturaInfoTributaria();
            infoFactura = new facturaInfoFactura();
            infoFactura.totalConImpuestos = new List<facturaInfoFacturaTotalImpuesto>();
            detalles = new List<facturaDetalle>();
            infoAdicional = new List<facturaCampoAdicional>();
        }
    }

    [GeneratedCodeAttribute("xsd", "4.0.30319.33440")]
    [SerializableAttribute()]
    [DebuggerStepThroughAttribute()]
    [DesignerCategoryAttribute("code")]
    [XmlTypeAttribute(AnonymousType = true)]
    public partial class facturaInfoTributaria
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
    }

    [GeneratedCodeAttribute("xsd", "4.0.30319.33440")]
    [SerializableAttribute()]
    [DebuggerStepThroughAttribute()]
    [DesignerCategoryAttribute("code")]
    [XmlTypeAttribute(AnonymousType = true)]
    public partial class facturaInfoFactura
    {
        public string fechaEmision { get; set; }
        public string dirEstablecimiento { get; set; }
        public string contribuyenteEspecial { get; set; }
        public string obligadoContabilidad { get; set; }
        public string tipoIdentificacionComprador { get; set; }
        public string guiaRemision { get; set; }
        public string razonSocialComprador { get; set; }
        public string identificacionComprador { get; set; }
        public string direccionComprador { get; set; }
        public string totalSinImpuestos { get; set; }
        public string totalDescuento { get; set; }
        [XmlArrayItemAttribute("totalImpuesto", IsNullable = false)]
        public List<facturaInfoFacturaTotalImpuesto> totalConImpuestos { get; set; }
        public string propina { get; set; }
        public string importeTotal { get; set; }
        public string moneda { get; set; }
        [XmlArrayItemAttribute("pago", IsNullable = false)]
        public List<facturaPago> pagos { get; set; }
        //[XmlIgnore]
        //public Double TarifaIVA
        //{
        //    get
        //    {
        //        double r = 0;
        //        foreach (facturaInfoFacturaTotalImpuesto i in totalConImpuestos)
        //        {
        //            if (r < i.tarifa)
        //                r = i.tarifa;
        //        }
        //        return r;
        //    }
        //}
        //[XmlIgnore]
        //public Double MontoIVA
        //{
        //    get
        //    {
        //        double r = 0;
        //        foreach (facturaInfoFacturaTotalImpuesto i in totalConImpuestos)
        //        {
        //            if (i.tarifa != 0)
        //                r = i.valor;
        //        }
        //        return r;
        //    }
        //}
    }

    [GeneratedCodeAttribute("xsd", "4.0.30319.33440")]
    [SerializableAttribute()]
    [DebuggerStepThroughAttribute()]
    [DesignerCategoryAttribute("code")]
    [XmlTypeAttribute(AnonymousType = true)]
    public partial class facturaInfoFacturaTotalImpuesto
    {
        public string codigo { get; set; }
        public string codigoPorcentaje { get; set; }
        public string descuentoAdicional { get; set; }
        public string baseImponible { get; set; }
        public string tarifa { get; set; }
        public string valor { get; set; }
    }

    [GeneratedCodeAttribute("xsd", "4.0.30319.33440")]
    [SerializableAttribute()]
    [DebuggerStepThroughAttribute()]
    [DesignerCategoryAttribute("code")]
    [XmlTypeAttribute(AnonymousType = true)]
    public partial class facturaPago
    {
        public string formaPago { get; set; }
        public string total { get; set; }
    }

    [GeneratedCodeAttribute("xsd", "4.0.30319.33440")]
    [SerializableAttribute()]
    [DebuggerStepThroughAttribute()]
    [DesignerCategoryAttribute("code")]
    [XmlTypeAttribute(AnonymousType = true)]
    public partial class facturaDetalle
    {
        public string codigoPrincipal { get; set; }
        public string codigoAuxiliar { get; set; }
        public string descripcion { get; set; }
        public string cantidad { get; set; }
        public string precioUnitario { get; set; }
        //public string precioUnitario
        //{
        //    get
        //    {
        //        return pUnit.ToString("F6").Replace(",", ".");
        //    }
        //    set { pUnit = Convert.ToDouble(value.Replace(".", ",")); }
        //}
        public string descuento { get; set; }
        public string precioTotalSinImpuesto { get; set; }
        public facturaDetalleImpuestos impuestos { get; set; }
    }

    [GeneratedCodeAttribute("xsd", "4.0.30319.33440")]
    [SerializableAttribute()]
    [DebuggerStepThroughAttribute()]
    [DesignerCategoryAttribute("code")]
    [XmlTypeAttribute(AnonymousType = true)]
    public partial class facturaDetalleImpuestos
    {
        public facturaDetalleImpuestosImpuesto impuesto { get; set; }
    }

    [GeneratedCodeAttribute("xsd", "4.0.30319.33440")]
    [SerializableAttribute()]
    [DebuggerStepThroughAttribute()]
    [DesignerCategoryAttribute("code")]
    [XmlTypeAttribute(AnonymousType = true)]
    public partial class facturaDetalleImpuestosImpuesto
    {

        public string codigo { get; set; }
        public string codigoPorcentaje { get; set; }
        public string tarifa { get; set; }
        public string baseImponible { get; set; }
        public string valor { get; set; }
    }

    /// <comentarios/>
    [GeneratedCodeAttribute("xsd", "4.0.30319.33440")]
    [SerializableAttribute()]
    [DebuggerStepThroughAttribute()]
    [DesignerCategoryAttribute("code")]
    [XmlTypeAttribute(AnonymousType = true)]
    public partial class facturaCampoAdicional
    {
        [XmlAttributeAttribute()]
        public string nombre { get; set; }
        [XmlTextAttribute()]
        public string Value { get; set; }
    }
}
