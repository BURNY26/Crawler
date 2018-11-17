using System;
using System.IO;
using EbayCrawlerWPF.Model;
using EbayCrawlerWPF.DataAccess;

namespace EbayCrawlerWPF.Controllers.Settings
{
    public static class SettingsController
    {
        private static bool _isInited = false;
        private static string _settingsXmlFilePath;

        private static ApplicationSettings _settings;
        public static ApplicationSettings Settings
        {
            get
            {
                if (!_isInited)
                {
                    Init();
                }

                return _settings;
            }
            set
            {
                if (value == _settings)
                {
                    return;
                }

                _settings = value;
            }
        }

        private static void Init()
        {
            SetFilePath();
            ReadSettings();

            _isInited = true;
        }

        private static void SetFilePath()
        {
            string directoryPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "EbayCrawler");
            Directory.CreateDirectory(directoryPath);

            _settingsXmlFilePath = Path.Combine(directoryPath, "settings.xml");
        }

        private static void ReadSettings()
        {
            Settings = XmlDataAccess.ReadXmlFile<ApplicationSettings>(_settingsXmlFilePath);
        }

        public static void SaveSettings()
        {
            XmlDataAccess.WriteToXml(Settings, _settingsXmlFilePath);
            Console.WriteLine(_settingsXmlFilePath);
        }
    }
}
