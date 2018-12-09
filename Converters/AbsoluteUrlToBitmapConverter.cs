using System;
using System.Globalization;
using System.Windows.Data;
using System.Windows.Media.Imaging;

namespace EbayCrawlerWPF.Converters
{
    public class AbsoluteUrlToBitmapConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            string uri = value.ToString();

            if (string.IsNullOrEmpty(uri))
            {
                return null;
            }

            BitmapImage bitmap = new BitmapImage();
            bitmap.BeginInit();
            bitmap.UriSource = new Uri(uri, UriKind.Absolute);
            bitmap.EndInit();

            return bitmap;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
