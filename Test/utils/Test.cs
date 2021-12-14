using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Test.utils
{

    // NOTE: Generated code may require at least .NET Framework 4.5 or .NET Core/Standard 2.0.
    /// <remarks/>
    [System.SerializableAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true)]
    [System.Xml.Serialization.XmlRootAttribute(Namespace = "", IsNullable = false)]
    public partial class factura
    {

        private facturaInfoTributaria infoTributariaField;

        private facturaInfoFactura infoFacturaField;

        private facturaDetalle[] detallesField;

        private facturaCampoAdicional[] infoAdicionalField;

        private string idField;

        private string versionField;

        /// <remarks/>
        public facturaInfoTributaria infoTributaria
        {
            get
            {
                return this.infoTributariaField;
            }
            set
            {
                this.infoTributariaField = value;
            }
        }

        /// <remarks/>
        public facturaInfoFactura infoFactura
        {
            get
            {
                return this.infoFacturaField;
            }
            set
            {
                this.infoFacturaField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlArrayItemAttribute("detalle", IsNullable = false)]
        public facturaDetalle[] detalles
        {
            get
            {
                return this.detallesField;
            }
            set
            {
                this.detallesField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlArrayItemAttribute("campoAdicional", IsNullable = false)]
        public facturaCampoAdicional[] infoAdicional
        {
            get
            {
                return this.infoAdicionalField;
            }
            set
            {
                this.infoAdicionalField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string id
        {
            get
            {
                return this.idField;
            }
            set
            {
                this.idField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string version
        {
            get
            {
                return this.versionField;
            }
            set
            {
                this.versionField = value;
            }
        }
    }

    /// <remarks/>
    [System.SerializableAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true)]
    public partial class facturaInfoTributaria
    {

        private byte ambienteField;

        private byte tipoEmisionField;

        private string razonSocialField;

        private string nombreComercialField;

        private ulong rucField;

        private string claveAccesoField;

        private byte codDocField;

        private byte estabField;

        private byte ptoEmiField;

        private uint secuencialField;

        private string dirMatrizField;

        /// <remarks/>
        public byte ambiente
        {
            get
            {
                return this.ambienteField;
            }
            set
            {
                this.ambienteField = value;
            }
        }

        /// <remarks/>
        public byte tipoEmision
        {
            get
            {
                return this.tipoEmisionField;
            }
            set
            {
                this.tipoEmisionField = value;
            }
        }

        /// <remarks/>
        public string razonSocial
        {
            get
            {
                return this.razonSocialField;
            }
            set
            {
                this.razonSocialField = value;
            }
        }

        /// <remarks/>
        public string nombreComercial
        {
            get
            {
                return this.nombreComercialField;
            }
            set
            {
                this.nombreComercialField = value;
            }
        }

        /// <remarks/>
        public ulong ruc
        {
            get
            {
                return this.rucField;
            }
            set
            {
                this.rucField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(DataType = "integer")]
        public string claveAcceso
        {
            get
            {
                return this.claveAccesoField;
            }
            set
            {
                this.claveAccesoField = value;
            }
        }

        /// <remarks/>
        public byte codDoc
        {
            get
            {
                return this.codDocField;
            }
            set
            {
                this.codDocField = value;
            }
        }

        /// <remarks/>
        public byte estab
        {
            get
            {
                return this.estabField;
            }
            set
            {
                this.estabField = value;
            }
        }

        /// <remarks/>
        public byte ptoEmi
        {
            get
            {
                return this.ptoEmiField;
            }
            set
            {
                this.ptoEmiField = value;
            }
        }

        /// <remarks/>
        public uint secuencial
        {
            get
            {
                return this.secuencialField;
            }
            set
            {
                this.secuencialField = value;
            }
        }

        /// <remarks/>
        public string dirMatriz
        {
            get
            {
                return this.dirMatrizField;
            }
            set
            {
                this.dirMatrizField = value;
            }
        }
    }

    /// <remarks/>
    [System.SerializableAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true)]
    public partial class facturaInfoFactura
    {

        private string fechaEmisionField;

        private string dirEstablecimientoField;

        private ushort contribuyenteEspecialField;

        private string obligadoContabilidadField;

        private byte tipoIdentificacionCompradorField;

        private string razonSocialCompradorField;

        private uint identificacionCompradorField;

        private decimal totalSinImpuestosField;

        private decimal totalDescuentoField;

        private facturaInfoFacturaTotalImpuesto[] totalConImpuestosField;

        private byte propinaField;

        private decimal importeTotalField;

        private string monedaField;

        private facturaInfoFacturaPagos pagosField;

        /// <remarks/>
        public string fechaEmision
        {
            get
            {
                return this.fechaEmisionField;
            }
            set
            {
                this.fechaEmisionField = value;
            }
        }

        /// <remarks/>
        public string dirEstablecimiento
        {
            get
            {
                return this.dirEstablecimientoField;
            }
            set
            {
                this.dirEstablecimientoField = value;
            }
        }

        /// <remarks/>
        public ushort contribuyenteEspecial
        {
            get
            {
                return this.contribuyenteEspecialField;
            }
            set
            {
                this.contribuyenteEspecialField = value;
            }
        }

        /// <remarks/>
        public string obligadoContabilidad
        {
            get
            {
                return this.obligadoContabilidadField;
            }
            set
            {
                this.obligadoContabilidadField = value;
            }
        }

        /// <remarks/>
        public byte tipoIdentificacionComprador
        {
            get
            {
                return this.tipoIdentificacionCompradorField;
            }
            set
            {
                this.tipoIdentificacionCompradorField = value;
            }
        }

        /// <remarks/>
        public string razonSocialComprador
        {
            get
            {
                return this.razonSocialCompradorField;
            }
            set
            {
                this.razonSocialCompradorField = value;
            }
        }

        /// <remarks/>
        public uint identificacionComprador
        {
            get
            {
                return this.identificacionCompradorField;
            }
            set
            {
                this.identificacionCompradorField = value;
            }
        }

        /// <remarks/>
        public decimal totalSinImpuestos
        {
            get
            {
                return this.totalSinImpuestosField;
            }
            set
            {
                this.totalSinImpuestosField = value;
            }
        }

        /// <remarks/>
        public decimal totalDescuento
        {
            get
            {
                return this.totalDescuentoField;
            }
            set
            {
                this.totalDescuentoField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlArrayItemAttribute("totalImpuesto", IsNullable = false)]
        public facturaInfoFacturaTotalImpuesto[] totalConImpuestos
        {
            get
            {
                return this.totalConImpuestosField;
            }
            set
            {
                this.totalConImpuestosField = value;
            }
        }

        /// <remarks/>
        public byte propina
        {
            get
            {
                return this.propinaField;
            }
            set
            {
                this.propinaField = value;
            }
        }

        /// <remarks/>
        public decimal importeTotal
        {
            get
            {
                return this.importeTotalField;
            }
            set
            {
                this.importeTotalField = value;
            }
        }

        /// <remarks/>
        public string moneda
        {
            get
            {
                return this.monedaField;
            }
            set
            {
                this.monedaField = value;
            }
        }

        /// <remarks/>
        public facturaInfoFacturaPagos pagos
        {
            get
            {
                return this.pagosField;
            }
            set
            {
                this.pagosField = value;
            }
        }
    }

    /// <remarks/>
    [System.SerializableAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true)]
    public partial class facturaInfoFacturaTotalImpuesto
    {

        private byte codigoField;

        private byte codigoPorcentajeField;

        private decimal descuentoAdicionalField;

        private decimal baseImponibleField;

        private decimal valorField;

        private decimal valorDevolucionIvaField;

        private bool valorDevolucionIvaFieldSpecified;

        /// <remarks/>
        public byte codigo
        {
            get
            {
                return this.codigoField;
            }
            set
            {
                this.codigoField = value;
            }
        }

        /// <remarks/>
        public byte codigoPorcentaje
        {
            get
            {
                return this.codigoPorcentajeField;
            }
            set
            {
                this.codigoPorcentajeField = value;
            }
        }

        /// <remarks/>
        public decimal descuentoAdicional
        {
            get
            {
                return this.descuentoAdicionalField;
            }
            set
            {
                this.descuentoAdicionalField = value;
            }
        }

        /// <remarks/>
        public decimal baseImponible
        {
            get
            {
                return this.baseImponibleField;
            }
            set
            {
                this.baseImponibleField = value;
            }
        }

        /// <remarks/>
        public decimal valor
        {
            get
            {
                return this.valorField;
            }
            set
            {
                this.valorField = value;
            }
        }

        /// <remarks/>
        public decimal valorDevolucionIva
        {
            get
            {
                return this.valorDevolucionIvaField;
            }
            set
            {
                this.valorDevolucionIvaField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlIgnoreAttribute()]
        public bool valorDevolucionIvaSpecified
        {
            get
            {
                return this.valorDevolucionIvaFieldSpecified;
            }
            set
            {
                this.valorDevolucionIvaFieldSpecified = value;
            }
        }
    }

    /// <remarks/>
    [System.SerializableAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true)]
    public partial class facturaInfoFacturaPagos
    {

        private facturaInfoFacturaPagosPago pagoField;

        /// <remarks/>
        public facturaInfoFacturaPagosPago pago
        {
            get
            {
                return this.pagoField;
            }
            set
            {
                this.pagoField = value;
            }
        }
    }

    /// <remarks/>
    [System.SerializableAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true)]
    public partial class facturaInfoFacturaPagosPago
    {

        private byte formaPagoField;

        private decimal totalField;

        private byte plazoField;

        private string unidadTiempoField;

        /// <remarks/>
        public byte formaPago
        {
            get
            {
                return this.formaPagoField;
            }
            set
            {
                this.formaPagoField = value;
            }
        }

        /// <remarks/>
        public decimal total
        {
            get
            {
                return this.totalField;
            }
            set
            {
                this.totalField = value;
            }
        }

        /// <remarks/>
        public byte plazo
        {
            get
            {
                return this.plazoField;
            }
            set
            {
                this.plazoField = value;
            }
        }

        /// <remarks/>
        public string unidadTiempo
        {
            get
            {
                return this.unidadTiempoField;
            }
            set
            {
                this.unidadTiempoField = value;
            }
        }
    }

    /// <remarks/>
    [System.SerializableAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true)]
    public partial class facturaDetalle
    {

        private ulong codigoPrincipalField;

        private ulong codigoAuxiliarField;

        private string descripcionField;

        private decimal cantidadField;

        private decimal precioUnitarioField;

        private decimal descuentoField;

        private decimal precioTotalSinImpuestoField;

        private facturaDetalleImpuestos impuestosField;

        /// <remarks/>
        public ulong codigoPrincipal
        {
            get
            {
                return this.codigoPrincipalField;
            }
            set
            {
                this.codigoPrincipalField = value;
            }
        }

        /// <remarks/>
        public ulong codigoAuxiliar
        {
            get
            {
                return this.codigoAuxiliarField;
            }
            set
            {
                this.codigoAuxiliarField = value;
            }
        }

        /// <remarks/>
        public string descripcion
        {
            get
            {
                return this.descripcionField;
            }
            set
            {
                this.descripcionField = value;
            }
        }

        /// <remarks/>
        public decimal cantidad
        {
            get
            {
                return this.cantidadField;
            }
            set
            {
                this.cantidadField = value;
            }
        }

        /// <remarks/>
        public decimal precioUnitario
        {
            get
            {
                return this.precioUnitarioField;
            }
            set
            {
                this.precioUnitarioField = value;
            }
        }

        /// <remarks/>
        public decimal descuento
        {
            get
            {
                return this.descuentoField;
            }
            set
            {
                this.descuentoField = value;
            }
        }

        /// <remarks/>
        public decimal precioTotalSinImpuesto
        {
            get
            {
                return this.precioTotalSinImpuestoField;
            }
            set
            {
                this.precioTotalSinImpuestoField = value;
            }
        }

        /// <remarks/>
        public facturaDetalleImpuestos impuestos
        {
            get
            {
                return this.impuestosField;
            }
            set
            {
                this.impuestosField = value;
            }
        }
    }

    /// <remarks/>
    [System.SerializableAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true)]
    public partial class facturaDetalleImpuestos
    {

        private facturaDetalleImpuestosImpuesto impuestoField;

        /// <remarks/>
        public facturaDetalleImpuestosImpuesto impuesto
        {
            get
            {
                return this.impuestoField;
            }
            set
            {
                this.impuestoField = value;
            }
        }
    }

    /// <remarks/>
    [System.SerializableAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true)]
    public partial class facturaDetalleImpuestosImpuesto
    {

        private byte codigoField;

        private byte codigoPorcentajeField;

        private decimal tarifaField;

        private decimal baseImponibleField;

        private decimal valorField;

        /// <remarks/>
        public byte codigo
        {
            get
            {
                return this.codigoField;
            }
            set
            {
                this.codigoField = value;
            }
        }

        /// <remarks/>
        public byte codigoPorcentaje
        {
            get
            {
                return this.codigoPorcentajeField;
            }
            set
            {
                this.codigoPorcentajeField = value;
            }
        }

        /// <remarks/>
        public decimal tarifa
        {
            get
            {
                return this.tarifaField;
            }
            set
            {
                this.tarifaField = value;
            }
        }

        /// <remarks/>
        public decimal baseImponible
        {
            get
            {
                return this.baseImponibleField;
            }
            set
            {
                this.baseImponibleField = value;
            }
        }

        /// <remarks/>
        public decimal valor
        {
            get
            {
                return this.valorField;
            }
            set
            {
                this.valorField = value;
            }
        }
    }

    /// <remarks/>
    [System.SerializableAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true)]
    public partial class facturaCampoAdicional
    {

        private string nombreField;

        private decimal valueField;

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string nombre
        {
            get
            {
                return this.nombreField;
            }
            set
            {
                this.nombreField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlTextAttribute()]
        public decimal Value
        {
            get
            {
                return this.valueField;
            }
            set
            {
                this.valueField = value;
            }
        }
    }


}
