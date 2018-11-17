using System;
using System.IO;
using System.Xml;
using EbayCrawlerWPF.Model;
using System.Xml.Serialization;

namespace EbayCrawlerWPF.Controllers.Settings
{
    public static class SettingsController
    {
        private static string _settingsXmlFilePath;

        private static ApplicationSettings _settings;
        public static ApplicationSettings Settings
        {
            get
            {
                return _settings;
            }
            set
            {
                _settings = value;
            }
        }


        public static void Init()
        {
            SetFilePath();
            ReadSettings();
        }

        private static void SetFilePath()
        {
            string directoryPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "EbayCrawler");
            Directory.CreateDirectory(directoryPath);

            _settingsXmlFilePath = Path.Combine(directoryPath, "settings.xml");
        }

        public static void ReadSettings()
        {
            Settings = ReadXmlFile<ApplicationSettings>(_settingsXmlFilePath);
        }

        public static void SaveSettings()
        {
            WriteToXml(Settings, _settingsXmlFilePath);
            Console.WriteLine(_settingsXmlFilePath);
        }

        private static T ReadXmlFile<T>(string filepath)
        {
            XmlSerializer serializer = new XmlSerializer(typeof(T));
            using (FileStream fileStream = new FileStream(filepath, FileMode.Open))
            {
                return (T)serializer.Deserialize(fileStream);
            }
        }

        private static void WriteToXml(object objectToSerialize, string filepath)
        {
            using (XmlWriter xmlWriter = XmlWriter.Create(filepath, new XmlWriterSettings { Indent = true, OmitXmlDeclaration = true }))
            {
                new XmlSerializer(objectToSerialize.GetType()).Serialize(xmlWriter, objectToSerialize, new XmlSerializerNamespaces(new[] { XmlQualifiedName.Empty }));
            }
        }
    }
}
