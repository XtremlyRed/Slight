using System;
using System.Globalization;
using System.Windows;
using System.Windows.Data;



namespace Slight.Core
{
    internal class String2VisibilityReConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return string.IsNullOrEmpty((string)value) ? Visibility.Visible : Visibility.Collapsed;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return value != null && (Visibility)value == Visibility.Collapsed;
        }
    }


    internal class String2VisibilityConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return string.IsNullOrEmpty((string)value) ? Visibility.Collapsed : Visibility.Visible;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return value != null && (Visibility)value == Visibility.Collapsed;
        }
    }


    internal class ValueIncreaseConverter : ValueConverterBase<double, int>
    {
        protected override object Convert(double value, Type targetType, int parameter, CultureInfo culture)
        {
            return value + parameter;
        }

        protected override object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
