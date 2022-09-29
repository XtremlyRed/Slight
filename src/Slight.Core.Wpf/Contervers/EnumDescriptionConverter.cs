using System;
using System.ComponentModel;
using System.Globalization;

namespace Slight.Core
{
    public class EnumDescriptionConverter : ValueConverterBase<Enum>
    {
        protected override object Convert(Enum value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value == null)
            {
                return string.Empty;
            }

            return value.GetAttribute<DescriptionAttribute>()?.Description ?? value.ToString();
        }
    }
}
