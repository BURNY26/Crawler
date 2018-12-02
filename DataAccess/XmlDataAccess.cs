using System.IO;
using System.Xml;
using System.Xml.Serialization;

namespace EbayCrawlerWPF.DataAccess
{
    public static class XmlDataAccess
    {
        public static void WriteToXml(object objectToSerialize, string filepath)
        {
            using (XmlWriter xmlWriter = XmlWriter.Create(filepath, new XmlWriterSettings { Indent = true, OmitXmlDeclaration = true }))
            {
                new XmlSerializer(objectToSerialize.GetType()).Serialize(xmlWriter, objectToSerialize, new XmlSerializerNamespaces(new[] { XmlQualifiedName.Empty }));
            }
        }

        public static T ReadXmlFile<T>(string filepath)
        {
            XmlSerializer serializer = new XmlSerializer(typeof(T));
            using (FileStream fileStream = new FileStream(filepath, FileMode.Open))
            {
                return (T)serializer.Deserialize(fileStream);
            }
        }

    }
}
