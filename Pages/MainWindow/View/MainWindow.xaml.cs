using System;
using System.Collections.Generic;
using System.IO;
using System.Windows;
using System.Xml;
using System.Xml.Serialization;
using EbayCrawlerWPF.Model;

namespace EbayCrawlerWPF
{
    public partial class MainWindow : Window
    {
        private Director _director;

        public MainWindow()
        {
            /*
            InitializeComponent();

            if (_director == null)
            {
                _director = new Director();
            }
            */

            
        }

        private void OnRunCrawlButtonClick(object sender, RoutedEventArgs e)
        {
            if (_director == null)
            {
                _director = new Director();
            }

            _director.Boot();
        }
    }
}
