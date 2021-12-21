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
        
        [XmlArrayItemAttribute("pago", IsNullable = false)]
        public List<facturaPago> pagos { get; set; }

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
            version = "1.1.0";
            id = "comprobante";
            infoTributaria = new facturaInfoTributaria();
            infoFactura = new facturaInfoFactura();
            infoFactura.totalConImpuestos = new List<facturaInfoFacturaTotalImpuesto>();
            pagos = new List<facturaPago>();
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
        public sbyte ambiente { get; set; }
        public sbyte tipoEmision { get; set; }
        public string razonSocial { get; set; }
        public string nombreComercial { get; set; }
        public string ruc { get; set; }
        public string claveAcceso { get; set; }
        public string codDoc { get; set; }
        public string estab { get; set; }
        public string ptoEmi { get; set; }
        public string secuencial { get; set; }
        public string dirMatriz { get; set; }
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
        public short contribuyenteEspecial { get; set; }
        public string obligadoContabilidad { get; set; }
        public string tipoIdentificacionComprador { get; set; }
        public string guiaRemision { get; set; }
        public string razonSocialComprador { get; set; }
        public string identificacionComprador { get; set; }
        public float totalSinImpuestos { get; set; }
        public float totalDescuento { get; set; }
        [XmlArrayItemAttribute("totalImpuesto", IsNullable = false)]
        public List<facturaInfoFacturaTotalImpuesto> totalConImpuestos { get; set; }
        public float propina { get; set; }
        public float importeTotal { get; set; }
        public string moneda { get; set; }
        [XmlIgnore]
        public Double TarifaIVA
        {
            get
            {
                double r = 0;
                foreach (facturaInfoFacturaTotalImpuesto i in totalConImpuestos)
                {
                    if (r < i.tarifa)
                        r = i.tarifa;
                }
                return r;
            }
        }
        [XmlIgnore]
        public Double MontoIVA
        {
            get
            {
                double r = 0;
                foreach (facturaInfoFacturaTotalImpuesto i in totalConImpuestos)
                {
                    if (i.tarifa != 0)
                        r = i.valor;
                }
                return r;
            }
        }
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
        public float descuentoAdicional { get; set; }
        public float baseImponible { get; set; }
        [XmlIgnore]
        public float tarifa { get; set; }
        public float valor { get; set; }
    }

    [GeneratedCodeAttribute("xsd", "4.0.30319.33440")]
    [SerializableAttribute()]
    [DebuggerStepThroughAttribute()]
    [DesignerCategoryAttribute("code")]
    [XmlTypeAttribute(AnonymousType = true)]
    public partial class facturaPago
    {
        public string formaPago { get; set; }
        public float importeTotal { get; set; }
    }

    [GeneratedCodeAttribute("xsd", "4.0.30319.33440")]
    [SerializableAttribute()]
    [DebuggerStepThroughAttribute()]
    [DesignerCategoryAttribute("code")]
    [XmlTypeAttribute(AnonymousType = true)]
    public partial class facturaDetalle
    {
        [XmlIgnore]
        private Double pUnit;
        public string codigoPrincipal { get; set; }
        public string codigoAuxiliar { get; set; }
        public string descripcion { get; set; }
        public float cantidad { get; set; }
        public string precioUnitario
        {
            get
            {
                return pUnit.ToString("F6").Replace(",", ".");
            }
            set { pUnit = Convert.ToDouble(value.Replace(".", ",")); }
        }
        public float descuento { get; set; }
        public float precioTotalSinImpuesto { get; set; }
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
        public float tarifa { get; set; }
        public float baseImponible { get; set; }
        public float valor { get; set; }
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
