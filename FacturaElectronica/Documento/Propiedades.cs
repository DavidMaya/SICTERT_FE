using System;
using System.Xml.Serialization;

namespace FacturaElectronica.Documento
{
    [Serializable]
    public enum AmbienteActivo
    {
        [XmlEnum("2")]
        Producción = 2,
        [XmlEnum("1")]
        Pruebas = 1
    }
    [Serializable]
    public enum TipoConsulta
    {
        [XmlEnum("1")]
        Consulta = 1,
        [XmlEnum("2")]
        Almacenamiento = 2
    }
    [Serializable]
    public enum TipoDocumento
    {
        [XmlEnum("1")]
        Factura = 1,
        [XmlEnum("2")]
        NotaCredito = 2,
        [XmlEnum("3")]
        Retencion = 3,
        [XmlEnum("4")]
        GuiaRemision = 4
    }

    [Serializable]
    public enum EstadoDocumento
    {
        [XmlEnum("SinFirma")]
        SinFirma = 1,
        [XmlEnum("MalEsquema")]
        MalEsquema = 5,
        [XmlEnum("Firmado")]
        Firmado = 2,

        [XmlEnum("Recibido")]
        Recibido = 3,
        [XmlEnum("Devuelto")]
        Devuelto = 7,

        [XmlEnum("Autorizado")]
        Autorizado = 4,
        [XmlEnum("Rechazado")]
        Rechazado = 6,
        [XmlEnum("EnviadoMail")]
        EnviadoMail = 7,

        [XmlEnum("EnviadoMailOffline")]
        EnviadoMailOffline = 8

    }

    [Serializable]
    public class consulta
    {
        public string Nombre { get; set; }
        public string Valor { get; set; }
        public TipoDocumento Tipo { get; set; }
        public EstadoDocumento Estado { get; set; }
        public TipoConsulta TipoConsulta { get; set; }
        public consulta()
        {
            Nombre = "";
            Valor = "";
            Tipo = TipoDocumento.Factura;
            Estado = EstadoDocumento.SinFirma;
        }
    }
}
