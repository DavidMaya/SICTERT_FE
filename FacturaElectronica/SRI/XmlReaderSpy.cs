using System.Xml;

namespace FacturaElectronica.SRI
{
    public class XmlReaderSpy : XmlReader
    {
        XmlReader reader;
        public XmlReaderSpy(XmlReader parent)
        {
            reader = parent;
        }

        public string Xml;

        #region Abstract method that must be implemented
        public override XmlNodeType NodeType
        {
            get
            {

                return reader.NodeType;
            }
        }

        public override string LocalName
        {
            get
            {
                return reader.LocalName;
            }
        }

        public override string NamespaceURI
        {
            get
            {
                return reader.NamespaceURI;
            }
        }

        public override string Prefix
        {
            get
            {
                return reader.Prefix;
            }
        }

        public override bool HasValue
        {
            get { return reader.HasValue; }
        }

        public override string Value
        {
            get { return reader.Value; }
        }

        public override int Depth
        {
            get { return reader.Depth; }
        }

        public override string BaseURI
        {
            get { return reader.BaseURI; }
        }

        public override bool IsEmptyElement
        {
            get { return reader.IsEmptyElement; }
        }

        public override int AttributeCount
        {
            get { return reader.AttributeCount; }
        }

        public override string GetAttribute(int i)
        {
            return reader.GetAttribute(i);
        }

        public override string GetAttribute(string name)
        {
            return reader.GetAttribute(name);
        }

        public override string GetAttribute(string name, string namespaceURI)
        {
            return reader.GetAttribute(name, namespaceURI);
        }

        public override void MoveToAttribute(int i)
        {
            reader.MoveToAttribute(i);
        }

        public override bool MoveToAttribute(string name)
        {
            return reader.MoveToAttribute(name);
        }

        public override bool MoveToAttribute(string name, string ns)
        {
            return reader.MoveToAttribute(name, ns);
        }

        public override bool MoveToFirstAttribute()
        {
            return reader.MoveToFirstAttribute();
        }

        public override bool MoveToNextAttribute()
        {
            return reader.MoveToNextAttribute();
        }

        public override bool MoveToElement()
        {
            return reader.MoveToElement();
        }

        public override bool ReadAttributeValue()
        {
            return reader.ReadAttributeValue();
        }

        public override bool Read()
        {
            bool res = reader.Read();

            Xml += StringView();


            return res;
        }

        public override bool EOF
        {
            get { return reader.EOF; }
        }

        public override void Close()
        {
            reader.Close();
        }

        public override ReadState ReadState
        {
            get { return reader.ReadState; }
        }

        public override XmlNameTable NameTable
        {
            get { return reader.NameTable; }
        }

        public override string LookupNamespace(string prefix)
        {
            return reader.LookupNamespace(prefix);
        }

        public override void ResolveEntity()
        {
            reader.ResolveEntity();
        }

        #endregion


        protected string StringView()
        {
            string result = "";

            if (reader.NodeType == XmlNodeType.Element)
            {
                result = "<" + reader.Name;

                if (reader.HasAttributes)
                {
                    reader.MoveToFirstAttribute();
                    do
                    {
                        result += " " + reader.Name + "=\"" + reader.Value + "\"";
                    } while (reader.MoveToNextAttribute());

                    //Let's put cursor back to Element to avoid messing up reader state.
                    reader.MoveToElement();
                }

                if (reader.IsEmptyElement)
                {
                    result += "/";
                }

                result += ">";
            }

            if (reader.NodeType == XmlNodeType.EndElement)
            {
                result = "</" + reader.Name + ">";
            }

            if (reader.NodeType == XmlNodeType.Text || reader.NodeType == XmlNodeType.Whitespace)
            {
                result = reader.Value;
            }



            if (reader.NodeType == XmlNodeType.XmlDeclaration)
            {
                result = "<?" + reader.Value + "?>";
            }

            return result;

        }
    }
}
