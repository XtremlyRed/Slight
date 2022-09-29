using System;
using System.Collections.ObjectModel;
using System.Globalization;
using System.Linq;
using System.Windows;
using System.Windows.Data;
using System.Windows.Markup;

namespace Slight.Core
{

    [ContentProperty(nameof(Cases))]
    public partial class SwitchExtension : MultiBinding
    {
        private Binding _to;
        private int _toIndex = Constants.InvalidIndex;

        [ConstructorArgument(nameof(To))]
        public Binding To
        {
            get => _to;
            set
            {
                if (_toIndex != Constants.InvalidIndex)
                {
                    throw new InvalidOperationException();
                }

                Bindings.Add(_to = value);
                _toIndex = Bindings.Count - 1;
            }
        }

        public CaseCollection Cases { get; }

        public SwitchExtension()
        {
            Cases = new CaseCollection(this);
            Converter = new MultiValueConverter(this);
        }

        private class MultiValueConverter : IMultiValueConverter
        {
            private readonly SwitchExtension _switchExtension;

            public MultiValueConverter(SwitchExtension switchExtension)
            {
                _switchExtension = switchExtension;
            }

            public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
            {
                object currentOption = values[_switchExtension._toIndex];
                if (currentOption == DependencyProperty.UnsetValue)
                {
                    return Binding.DoNothing;
                }

                CaseExtension @case = _switchExtension.Cases.FirstOrDefault(item => Equals(currentOption, item.Label)) ??
                            _switchExtension.Cases.FirstOrDefault(item => Equals(Constants.DefaultLabel, item.Label));

                if (@case == null)
                {
                    return null;
                }

                int index = @case.Index;
                return index == Constants.InvalidIndex ? @case.Value : values[index];
            }

            public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
            {
                throw new NotSupportedException();
            }
        }
    }

    internal static class Constants
    {
        public const int InvalidIndex = -1;

        public static readonly object DefaultLabel = new();
    }

    [MarkupExtensionReturnType(typeof(CaseExtension))]
    [ContentProperty(nameof(Value))]
    public class CaseExtension : MarkupExtension
    {
        internal int Index { get; set; } = Constants.InvalidIndex;

        [ConstructorArgument(nameof(Label))]
        public object Label { get; set; } = Constants.DefaultLabel;

        [ConstructorArgument(nameof(Value))]
        public object Value { get; set; }

        public CaseExtension() { }

        public CaseExtension(object value)
        {
            Value = value;
        }

        public CaseExtension(object option, object value)
        {
            Label = option;
            Value = value;
        }

        public override object ProvideValue(IServiceProvider serviceProvider)
        {
            return this;
        }
    }


    public class CaseCollection : Collection<CaseExtension>
    {
        private readonly SwitchExtension _switchExtension;

        public CaseCollection(SwitchExtension switchExtension)
        {
            _switchExtension = switchExtension;
        }

        protected override void InsertItem(int index, CaseExtension item)
        {
            if (ReferenceEquals(item.Label, Constants.DefaultLabel) &&
                Items.Any(it => ReferenceEquals(it.Label, Constants.DefaultLabel)))
            {
                throw new InvalidOperationException(
                    "A Switch markup extension must not contain more than one default Case markup extension.");
            }

            if (item.Value is BindingBase binding)
            {
                _switchExtension.Bindings.Add(binding);
                item.Index = _switchExtension.Bindings.Count - 1;
            }
            else
            {
                base.InsertItem(index, item);
            }
        }
    }
}
