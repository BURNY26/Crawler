using System.Windows;
using EbayCrawlerWPF.Model;

namespace EbayCrawlerWPF
{
    public partial class MainWindow : Window
    {
        private Director _director;

        public MainWindow()
        {
            //InitializeComponent();
            
            if (_director == null)
            {
                _director = new Director();
            }

            _director.Boot();

        }

        private void OnRunCrawlButtonClick(object sender, RoutedEventArgs e)
        {
            if (_director == null)
            {
                _director = new Director();
            }

            _director.Boot();
            //_director.ExportHtmlToCsv("ebay191bug.html", "slechteitems.csv");
        }
    }
}
