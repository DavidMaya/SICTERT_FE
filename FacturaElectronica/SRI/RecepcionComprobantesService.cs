using FacturaElectronica.wsRecepcionSRI;
using System.Xml;
using System.Web.Services.Protocols;

namespace FacturaElectronica.SRI
{
    public class RecepcionComprobantesService1 : RecepcionComprobantesService
    {
        private XmlWriterSpy writer;
        private XmlReaderSpy reader;

        public string XmlRequest { get { return (writer == null ? null : writer.Xml); } }

        public string XmlResponse { get { return (reader == null ? string.Empty : reader.Xml); } }

        protected override XmlWriter GetWriterForMessage(SoapClientMessage message, int bufferSize)
        {
            writer = new XmlWriterSpy(base.GetWriterForMessage(message, bufferSize));
            return writer;
        }

        protected override XmlReader GetReaderForMessage(SoapClientMessage message, int bufferSize)
        {
            XmlReader rdr = base.GetReaderForMessage(message, bufferSize);
            reader = new XmlReaderSpy((XmlReader)rdr);
            return reader;

        }

    }
}
