using System;
using System.Globalization;
using System.Windows.Data;



namespace Slight.Core
{
    public abstract class ValueConverterBase<TValueType, TParameterType> : IValueConverter
    {
        protected abstract object Convert(TValueType value, Type targetType, TParameterType parameter, CultureInfo culture);

        object IValueConverter.Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (Invoker.TryCast(value, out TValueType tvalue) == false)
            {
                if (ConvertValueTypeError(value, out tvalue) == false)
                {
                    tvalue = default;
                }
            }

            if (Invoker.TryCast(parameter, out TParameterType tparameter) == false)
            {
                if (ConvertParameterTypeError(value, out tparameter) == false)
                {
                    tparameter = default;
                }
            }

            return Convert(tvalue, targetType, tparameter, culture);
        }

        object IValueConverter.ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return ConvertBack(value, targetType, parameter, culture);
        }

        protected virtual object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }

        protected virtual bool ConvertValueTypeError(object currentValue, out TValueType value)
        {
            value = default;
            return false;
        }
        protected virtual bool ConvertParameterTypeError(object currentParameter, out TParameterType Parametervalue)
        {
            Parametervalue = default;
            return false;
        }
    }


    public abstract class ValueConverterBase<TValueType> : IValueConverter
    {
        protected abstract object Convert(TValueType value, Type targetType, object parameter, CultureInfo culture);

        object IValueConverter.Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (Invoker.TryCast(value, out TValueType tvalue) == false)
            {
                if (ConvertValueTypeError(value, out tvalue) == false)
                {
                    tvalue = default;
                }
            }


            return Convert(tvalue, targetType, parameter, culture);
        }

        object IValueConverter.ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return ConvertBack(value, targetType, parameter, culture);
        }

        protected virtual object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }

        protected virtual bool ConvertValueTypeError(object currentValue, out TValueType valueType)
        {
            valueType = default;
            return false;
        }
    }
}
