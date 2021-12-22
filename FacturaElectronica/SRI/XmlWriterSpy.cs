using System.IO;
using System.Xml;

namespace FacturaElectronica.SRI
{
    public class XmlWriterSpy : XmlWriter
    {
        private XmlWriter writer;
        private XmlTextWriter buffer;
        private StringWriter stringWriter;

        public XmlWriterSpy(XmlWriter implementation)
        {
            writer = implementation;
            stringWriter = new StringWriter();
            buffer = new XmlTextWriter(stringWriter);
            buffer.Formatting = Formatting.Indented;
        }
        public override void Flush()
        {
            writer.Flush();
            buffer.Flush();
            stringWriter.Flush();
        }
        public string Xml { get { return (stringWriter == null ? null : stringWriter.ToString()); } }

        public override void Close() { writer.Close(); buffer.Close(); }
        public override string LookupPrefix(string ns) { return writer.LookupPrefix(ns); }
        public override void WriteBase64(byte[] buffer, int index, int count) { writer.WriteBase64(buffer, index, count); this.buffer.WriteBase64(buffer, index, count); }

        public override WriteState WriteState
        {
            get { return writer.WriteState; }
        }

        public override void WriteRaw(string data)
        {
            writer.WriteRaw(data);
        }

        public override void WriteRaw(char[] buffer, int index, int count)
        {
            writer.WriteRaw(buffer, index, count);
        }

        public override void WriteChars(char[] buffer, int index, int count)
        {
            writer.WriteChars(buffer, index, count);
        }

        public override void WriteString(string text)
        {
            writer.WriteString(text);
        }

        public override void WriteCharEntity(char ch)
        {
            writer.WriteCharEntity(ch);
        }

        public override void WriteEntityRef(string name)
        {
            writer.WriteEntityRef(name);
        }

        public override void WriteSurrogateCharEntity(char lowChar, char highChar) { writer.WriteSurrogateCharEntity(lowChar, highChar); buffer.WriteSurrogateCharEntity(lowChar, highChar); }
        public override void WriteWhitespace(string ws) { writer.WriteWhitespace(ws); buffer.WriteWhitespace(ws); }


        public override void WriteCData(string text)
        {
            writer.WriteCData(text);
        }

        public override void WriteComment(string text)
        {
            writer.WriteComment(text);
        }

        public override void WriteDocType(string name, string pubid, string sysid, string subset)
        {
            writer.WriteDocType(name, pubid, sysid, subset);
        }

        public override void WriteEndAttribute()
        {
            writer.WriteEndAttribute();
        }

        public override void WriteEndDocument()
        {
            writer.WriteEndDocument();
        }

        public override void WriteEndElement()
        {
            writer.WriteEndElement();
        }

        public override void WriteFullEndElement()
        {
            writer.WriteFullEndElement();
        }

        public override void WriteProcessingInstruction(string name, string text)
        {
            writer.WriteProcessingInstruction(name, text);
        }

        public override void WriteStartAttribute(string prefix, string localName, string ns)
        {
            writer.WriteStartAttribute(prefix, localName, ns);
        }

        public override void WriteStartDocument(bool standalone)
        {
            writer.WriteStartDocument(standalone);
        }

        public override void WriteStartDocument()
        {
            writer.WriteStartDocument();
        }

        public override void WriteStartElement(string prefix, string localName, string ns)
        {
            writer.WriteStartElement(prefix, localName, ns);
        }

        /*protected override void Dispose(bool disposing)
        {
            base.Dispose(disposing);

            _me.Close();
        }*/
    }
}
