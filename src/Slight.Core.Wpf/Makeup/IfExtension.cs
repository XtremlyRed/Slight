using System;
using System.Globalization;
using System.Windows;
using System.Windows.Data;
using System.Windows.Markup;

namespace Slight.Core
{
    public class IfExtension : MultiBinding
    {
        private const int InvalidIndex = -1;
        private int _conditionIndex = InvalidIndex;
        private int _trueIndex = InvalidIndex;
        private int _falseIndex = InvalidIndex;

        private object _true;
        private object _false;

        [ConstructorArgument(nameof(Condition))]
        public Binding Condition
        {
            set => SetProperty(value, ref _conditionIndex, out _);
        }

        public object Value { internal get; set; }

        [ConstructorArgument(nameof(True))]
        public object True
        {
            set => SetProperty(value, ref _trueIndex, out _true);
        }

        [ConstructorArgument(nameof(False))]
        public object False
        {
            set => SetProperty(value, ref _falseIndex, out _false);
        }

        public IfExtension()
        {
            Converter = new MultiValueConverter(this);
        }

        public IfExtension(Binding condition, object trueValue, object falseValue) : this()
        {
            Condition = condition;
            True = trueValue;
            False = falseValue;
        }

        private void SetProperty<Target>(Target value, ref int index, out Target storage)
        {
            if (index != InvalidIndex)
            {
                throw new InvalidOperationException("Cannot reset the value. ");
            }

            if (value is BindingBase binding)
            {
                Bindings.Add(binding);
                index = Bindings.Count - 1;
                storage = default;
            }
            else
            {
                storage = value;
            }
        }

        private class MultiValueConverter : IMultiValueConverter
        {
            private readonly IfExtension _ifExtension;

            public MultiValueConverter(IfExtension ifExtension)
            {
                _ifExtension = ifExtension;
            }

            public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
            {
                object condition = values[_ifExtension._conditionIndex];

                object value = _ifExtension.Value;

                if (condition == DependencyProperty.UnsetValue || value == DependencyProperty.UnsetValue)
                {
                    return Binding.DoNothing;
                }

                bool flag = false;

                if (condition != null && value != null)
                {
                    flag = condition.GetHashCode() == value.GetHashCode();
                }

                return flag
                    ? GetValue(_ifExtension._trueIndex, _ifExtension._true)
                    : GetValue(_ifExtension._falseIndex, _ifExtension._false);

                object GetValue(int index, object storage)
                {
                    return index != InvalidIndex ? values[index] : storage;
                }
            }

            public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
            {
                throw new NotSupportedException();
            }
        }
    }
}
