
using System;
using System.Globalization;
using System.Linq;
using System.Windows;
namespace Slight.Core
{
    /// <summary>
    /// 
    /// </summary>
    public class ThicknessIncreaseConverter : ValueConverterBase<Thickness, string>
    {
        public static readonly char[] ConnectChar = new char[] { ',', '，', '_', '`', '~', '*', '/', '^', '`', '·', '\'', ';', '；', ':', '：', '\\' };

        protected override object Convert(Thickness value, Type targetType, string parameter, CultureInfo culture)
        {
            string[] chars = parameter?.Split(ConnectChar, StringSplitOptions.RemoveEmptyEntries).Take(4).ToArray();

            if (chars is null || chars.Length == 0)
            {
                return value;
            }

            double l = 0d, t = 0d, r = 0d, b = 0d;

            int index = 0;
            foreach (string item in chars)
            {
                switch (index)
                {
                    case 0: l = ParseValue(item); break;
                    case 1: t = ParseValue(item); break;
                    case 2: r = ParseValue(item); break;
                    case 3: b = ParseValue(item); break;
                    default: break;
                }
                index++;
            }

            return new Thickness(l + value.Left, t + value.Top, r + value.Right, b + value.Bottom);


            static double ParseValue(string stringValue)
            {
                if (string.IsNullOrWhiteSpace(stringValue))
                {
                    return 0d;
                }
                double.TryParse(stringValue, out double dougleValue);
                return dougleValue;
            }
        }

        protected override object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
