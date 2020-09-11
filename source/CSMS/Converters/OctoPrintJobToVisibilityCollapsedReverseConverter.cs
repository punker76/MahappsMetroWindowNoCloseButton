using System;
using System.Globalization;
using System.Windows;
using System.Windows.Data;

namespace WpfFramework.Converters
{
    public sealed class OctoPrintJobToVisibilityCollapsedReverseConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return Visibility.Collapsed;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
