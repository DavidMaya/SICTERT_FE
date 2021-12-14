using System;
using System.IO;
using System.Text;
using System.Xml;
using System.Xml.Serialization;

namespace Test.utils
{
    public static class XmlTools
    {
        public static T Deserialize<T>(string Serializado)
        {
            object r = null;
            try
            {
                XmlSerializer mySerializer;
                Type t = typeof(T);
                mySerializer = new XmlSerializer(typeof(T));
                using (TextReader reader = new StringReader(Serializado))
                {
                    r = mySerializer.Deserialize(reader);
                }
            }
            catch (Exception)
            {
                r = new object();
            }
            return (T)r;
        }

        public static string Serialize<T>(this T value)
        {
            if (value == null) return string.Empty;

            try
            {
                var xmlSerialize = new XmlSerializer(typeof(T));
                XmlSerializerNamespaces namespaces = new XmlSerializerNamespaces();
                namespaces.Add("", "");
                var stringWriter = new Utf8StringWriter();
                using (var writer = XmlWriter.Create(stringWriter))
                {
                    xmlSerialize.Serialize(writer, value, namespaces);
                    return stringWriter.ToString();
                }
            }
            catch (Exception)
            {
                return "";
            }
        }

        private class Utf8StringWriter : StringWriter
        {
            public override Encoding Encoding => Encoding.UTF8;
        }
    }
}
