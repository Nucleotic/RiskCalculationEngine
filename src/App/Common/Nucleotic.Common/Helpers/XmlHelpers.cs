using System.IO;
using System.Xml;
using System.Xml.Serialization;

namespace Nucleotic.Common.Helpers
{
    public static class XmlHelpers
    {
        /// <summary>
        /// Serializes the object.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="xml">The XML.</param>
        /// <returns></returns>
        public static T SerializeObject<T>(string xml) where T : new()
        {
            var result = new T();
            if (string.IsNullOrEmpty(xml)) return result;
            using (TextReader reader = new StringReader(xml))
            {
                var serializer = new XmlSerializer(typeof(T));
                result = (T)serializer.Deserialize(reader);
            }
            return result;
        }

        /// <summary>
        /// Deserializes the inner SOAP object.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="soapResponse">The SOAP response.</param>
        /// <returns></returns>
        public static T DeserializeInnerSoapObject<T>(string soapResponse)
        {
            XmlDocument xmlDocument = new XmlDocument();
            xmlDocument.LoadXml(soapResponse);

            var soapBody = xmlDocument.GetElementsByTagName("soap:Body")[0];
            string innerObject = soapBody.InnerXml;

            XmlSerializer deserializer = new XmlSerializer(typeof(T));

            using (StringReader reader = new StringReader(innerObject))
            {
                return (T)deserializer.Deserialize(reader);
            }
        }

        /// <summary>
        /// Deserializes the XML file to object.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="xmlFilename">The XML filename.</param>
        /// <returns></returns>
        public static T DeserializeXmlFileToObject<T>(string xmlFilename)
        {
            if (string.IsNullOrEmpty(xmlFilename)) return default(T);

            StreamReader xmlStream = new StreamReader(xmlFilename);
            XmlSerializer serializer = new XmlSerializer(typeof(T));
            var returnObject = (T)serializer.Deserialize(xmlStream);
            return returnObject;
        }

        /// <summary>
        /// Returns a <see cref="System.String" /> that represents this instance.
        /// </summary>
        /// <param name="node">The node.</param>
        /// <param name="indentation">The indentation.</param>
        /// <returns>
        /// A <see cref="System.String" /> that represents this instance.
        /// </returns>
        public static string ToString(this XmlNode node, int indentation)
        {
            using (var sw = new StringWriter())
            {
                using (var xw = new XmlTextWriter(sw))
                {
                    xw.Formatting = Formatting.Indented;
                    xw.Indentation = indentation;
                    node.WriteContentTo(xw);
                }
                return sw.ToString();
            }
        }
    }
}
