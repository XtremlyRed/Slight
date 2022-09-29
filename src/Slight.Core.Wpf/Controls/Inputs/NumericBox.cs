
using System;
using System.ComponentModel;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
namespace Slight.Core
{

    public abstract class NumericBoxBase : Control
    {

        private System.Windows.Controls.TextBox textBox;
        //= new System.Windows.Controls.TextBox()
        //{
        //    BorderBrush = Brushes.Transparent,
        //    BorderThickness = new Thickness(0),
        //    Background = Brushes.Transparent,
        //    VerticalContentAlignment = VerticalAlignment.Center,
        //    HorizontalContentAlignment = HorizontalAlignment.Left,
        //    VerticalAlignment = VerticalAlignment.Stretch,
        //    HorizontalAlignment = HorizontalAlignment.Stretch,
        //    Padding = new Thickness(5, 0, 5, 0),
        //};
        internal const string ClickDown = "ClickDown";
        internal const string ClickUp = "ClickUp";
        internal const string TextBoxToken = "TextBoxToken";
        private bool trigger = true;

        public static readonly DependencyProperty IncrementVisibilityProperty =
            PropertyAssist.PropertyRegister<NumericBoxBase, Visibility>(i => i.IncrementVisibility, Visibility.Visible, (s, e) =>
            {
                s.IncrementVisibility = e.NewValue;
            });
        [Bindable(true), Category("Increment")]
        [Localizability(LocalizationCategory.None, Readability = Readability.Unreadable)]
        public Visibility IncrementVisibility
        {
            get => (Visibility)GetValue(IncrementVisibilityProperty);
            set => SetValue(IncrementVisibilityProperty, value);
        }

        public static readonly DependencyProperty IncrementForegroundProperty =
            PropertyAssist.PropertyRegister<NumericBoxBase, Brush>(i => i.IncrementForeground, Brushes.Black);
        [Bindable(true), Category("Increment")]
        [Localizability(LocalizationCategory.None, Readability = Readability.Unreadable)]
        public Brush IncrementForeground
        {
            get => (Brush)GetValue(IncrementForegroundProperty);
            set => SetValue(IncrementForegroundProperty, value);
        }
        public static readonly DependencyProperty SelectionTextBrushProperty =
              PropertyAssist.PropertyRegister<NumericBoxBase, Brush>(i => i.SelectionTextBrush, Brushes.LightGray);
        [Bindable(true), Category("Caret")]
        [Localizability(LocalizationCategory.None, Readability = Readability.Unreadable)]
        public Brush SelectionTextBrush
        {
            get => (Brush)GetValue(SelectionTextBrushProperty);
            set => SetValue(SelectionTextBrushProperty, value);
        }

        public static readonly DependencyProperty CaretBrushProperty =
          PropertyAssist.PropertyRegister<NumericBoxBase, Brush>(i => i.CaretBrush, Brushes.Black);
        [Bindable(true), Category("Caret")]
        [Localizability(LocalizationCategory.None, Readability = Readability.Unreadable)]
        public Brush CaretBrush
        {
            get => (Brush)GetValue(CaretBrushProperty);
            set => SetValue(CaretBrushProperty, value);
        }

        public static readonly DependencyProperty CaretIndexProperty =
         PropertyAssist.PropertyRegister<NumericBoxBase, int>(i => i.CaretIndex, 0, (s, e) =>
         {
             s.textBox.CaretIndex = e.NewValue.FromRange(0, int.MaxValue);
         });
        [Bindable(true), Category("Caret")]
        [Localizability(LocalizationCategory.None, Readability = Readability.Unreadable)]
        public int CaretIndex
        {
            get => (int)GetValue(CaretIndexProperty);
            set => SetValue(CaretIndexProperty, value);
        }



        public static readonly DependencyProperty HexModeProperty =
        PropertyAssist.PropertyRegister<NumericBoxBase, HexMode>(i => i.HexMode, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault | FrameworkPropertyMetadataOptions.AffectsRender, (s, e) =>
        {
            if (e.NewValue == e.OldValue)
            {
                return;
            }

            s.Display(false);
        });

        [Bindable(true), Category("Value")]
        [Localizability(LocalizationCategory.None, Readability = Readability.Unreadable)]
        public HexMode HexMode
        {
            get => (HexMode)GetValue(HexModeProperty);
            set => SetValue(HexModeProperty, value);
        }

        public static readonly DependencyProperty CornerRadiusProperty = PropertyAssist.PropertyRegister<NumericBoxBase, CornerRadius>(i => i.CornerRadius, new CornerRadius(0));

        [Bindable(true), Category("CornerRadius")]
        [Localizability(LocalizationCategory.None, Readability = Readability.Unreadable)]
        public CornerRadius CornerRadius
        {
            get => (CornerRadius)GetValue(CornerRadiusProperty);
            set => SetValue(CornerRadiusProperty, value);
        }

        public static new readonly DependencyProperty PaddingProperty = PropertyAssist.PropertyRegister<NumericBoxBase, Thickness>(i => i.Padding, new Thickness(0));

        [Bindable(true), Category("Thickness")]
        [Localizability(LocalizationCategory.None, Readability = Readability.Unreadable)]
        public new Thickness Padding
        {
            get => (Thickness)GetValue(PaddingProperty);
            set => SetValue(PaddingProperty, value);
        }


        public override void OnApplyTemplate()
        {
            base.OnApplyTemplate();

            if (GetTemplateChild(ClickDown) is Grid clickDown)
            {
                clickDown.MouseLeftButtonUp += (s, e) => IncrementInterop(false);
            }
            if (GetTemplateChild(ClickUp) is Grid clickUp)
            {
                clickUp.MouseLeftButtonUp += (s, e) => IncrementInterop(true);
            }

            if (GetTemplateChild(TextBoxToken) is System.Windows.Controls.TextBox cc)
            {
                textBox = cc;
                textBox.CaretIndex = CaretIndex;
                InputMethod.SetIsInputMethodEnabled(textBox, false);

                Display(false);

                textBox.TextChanged += (s, e) =>
                {
                    if (trigger)
                    {
                        CurrentValueChanged(textBox.Text);
                    }
                };

                textBox.LostFocus += (s, e) => LostFocus?.Invoke(this, e);

            }
        }

        public new event EventHandler<RoutedEventArgs> LostFocus;



        protected NumericBoxBase()
        {
        }

        internal abstract void CurrentValueChanged(string currentValue);
        protected void Display(bool needTrigger)
        {
            string newValue = ToDisplayString();
            if (textBox != null && textBox.Text != newValue)
            {
                try
                {
                    trigger = needTrigger;
                    textBox.Text = newValue;
                }
                finally
                {
                    trigger = true;
                }
            }
        }
        internal abstract void IncrementInterop(bool isAdd);

        internal abstract string ToDisplayString();
    }


    public abstract class NumericBox<TValue> : NumericBoxBase
        where TValue : IFormattable, IComparable<TValue>
    {

        #region  Increment

        public static readonly DependencyProperty IncrementProperty =
            PropertyAssist.PropertyRegister<NumericBox<TValue>, TValue>(i => i.Increment, default(TValue));
        [Bindable(true), Category("Increment")]
        [Localizability(LocalizationCategory.None, Readability = Readability.Unreadable)]
        public TValue Increment
        {
            get => (TValue)GetValue(IncrementProperty);
            set => SetValue(IncrementProperty, value);
        }

        #endregion

        #region  Value

        public static readonly DependencyProperty ValueProperty = PropertyAssist.PropertyRegister<NumericBox<TValue>, TValue>(i => i.Value, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault | FrameworkPropertyMetadataOptions.AffectsRender, (s, e) =>
          {
              if (Equals(e.NewValue, e.OldValue) == false)
              {
                  s.Display(false);
              }
          });
        [Bindable(true), Category("Value")]
        [Localizability(LocalizationCategory.None, Readability = Readability.Unreadable)]
        public TValue Value
        {
            get => (TValue)GetValue(ValueProperty);
            set => SetValue(ValueProperty, value);
        }

        public static readonly DependencyProperty MinValueProperty = PropertyAssist.PropertyRegister<NumericBox<TValue>, TValue>(i => i.MinValue);
        [Bindable(true), Category("Value")]
        [Localizability(LocalizationCategory.None, Readability = Readability.Unreadable)]
        public TValue MinValue
        {
            get => (TValue)GetValue(MinValueProperty);
            set => SetValue(MinValueProperty, value);
        }

        public static readonly DependencyProperty MaxValueProperty = PropertyAssist.PropertyRegister<NumericBox<TValue>, TValue>(i => i.MaxValue);
        [Bindable(true), Category("Value")]
        [Localizability(LocalizationCategory.None, Readability = Readability.Unreadable)]
        public TValue MaxValue
        {
            get => (TValue)GetValue(MaxValueProperty);
            set => SetValue(MaxValueProperty, value);
        }

        #endregion
        internal override void CurrentValueChanged(string currentValue)
        {
            Value = ToTValue(currentValue);
        }
        internal override string ToDisplayString()
        {
            if (HexMode == HexMode.Ten)
            {
                return Value.ToString();
            }

            return $"0x{Value:X2}";//  Value.ToString("X2", null);
        }

        protected abstract TValue ToTValue(string stringValue);

        internal override void IncrementInterop(bool isAdd)
        {
            TValue current = Value;
            TValue increment = Increment;
            if (isAdd)
            {
                TValue reduce = Interop(MaxValue, increment, false);
                Value = reduce.CompareTo(current) > 0 ? Interop(current, increment, true) : MaxValue;
                return;
            }
            TValue reduce2 = Interop(MinValue, increment, true);
            Value = current.CompareTo(reduce2) > 0 ? Interop(current, increment, false) : MinValue;
        }

        internal abstract TValue Interop(TValue value1, TValue value2, bool isAdd);
    }

    public class UShortInputBox : NumericBox<ushort>
    {
        public UShortInputBox()
        {
            MaxValue = ushort.MaxValue;
            MinValue = ushort.MinValue;
        }

        protected override ushort ToTValue(string valueString)
        {
            if (HexMode == HexMode.Ten)
            {
                return Convert.ToUInt16(valueString);
            }
            return Convert.ToUInt16(valueString, 16);
        }

        internal override ushort Interop(ushort value1, ushort value2, bool isAdd)
        {
            return isAdd ? (ushort)(value1 + value2) : (ushort)(value1 - value2);
        }
    }

    public class ShortInputBox : NumericBox<short>
    {
        public ShortInputBox()
        {
            MaxValue = short.MaxValue;
            MinValue = short.MinValue;
        }
        internal override short Interop(short value1, short value2, bool isAdd)
        {
            return isAdd ? (short)(value1 + value2) : (short)(value1 - value2);
        }

        protected override short ToTValue(string valueString)
        {
            if (HexMode == HexMode.Ten)
            {
                return Convert.ToInt16(valueString);
            }
            return Convert.ToInt16(valueString, 16);

        }
    }

    public class UIntInputBox : NumericBox<uint>
    {
        public UIntInputBox()
        {
            MaxValue = uint.MaxValue;
            MinValue = uint.MinValue;
        }
        internal override uint Interop(uint value1, uint value2, bool isAdd)
        {
            return isAdd ? value1 + value2 : value1 - value2;
        }

        protected override uint ToTValue(string valueString)
        {
            if (HexMode == HexMode.Ten)
            {
                return Convert.ToUInt32(valueString);
            }
            return Convert.ToUInt32(valueString, 16);

        }
    }

    public class IntInputBox : NumericBox<int>
    {
        public IntInputBox()
        {
            MaxValue = int.MaxValue;
            MinValue = int.MinValue;
        }
        internal override int Interop(int value1, int value2, bool isAdd)
        {
            return isAdd ? value1 + value2 : value1 - value2;
        }

        protected override int ToTValue(string valueString)
        {
            if (HexMode == HexMode.Ten)
            {
                return Convert.ToInt32(valueString);
            }
            return Convert.ToInt32(valueString, 16);
        }
    }

    public class ULongInputBox : NumericBox<ulong>
    {
        public ULongInputBox()
        {
            MaxValue = ulong.MaxValue;
            MinValue = ulong.MinValue;
        }
        internal override ulong Interop(ulong value1, ulong value2, bool isAdd)
        {
            return isAdd ? value1 + value2 : value1 - value2;
        }

        protected override ulong ToTValue(string valueString)
        {
            if (HexMode == HexMode.Ten)
            {
                return Convert.ToUInt64(valueString);
            }
            return Convert.ToUInt64(valueString, 16);
        }
    }

    public class LongInputBox : NumericBox<long>
    {
        public LongInputBox()
        {
            MaxValue = long.MaxValue;
            MinValue = long.MinValue;
        }
        internal override long Interop(long value1, long value2, bool isAdd)
        {
            return isAdd ? value1 + value2 : value1 - value2;
        }

        protected override long ToTValue(string valueString)
        {
            if (HexMode == HexMode.Ten)
            {
                return Convert.ToInt64(valueString);
            }
            return Convert.ToInt64(valueString, 16);
        }
    }

    public class SByteInputBox : NumericBox<sbyte>
    {
        public SByteInputBox()
        {
            MaxValue = sbyte.MaxValue;
            MinValue = sbyte.MinValue;
        }
        internal override sbyte Interop(sbyte value1, sbyte value2, bool isAdd)
        {
            return isAdd ? (sbyte)(value1 + value2) : (sbyte)(value1 - value2);
        }

        protected override sbyte ToTValue(string valueString)
        {
            if (HexMode == HexMode.Ten)
            {
                return Convert.ToSByte(valueString);
            }
            return Convert.ToSByte(valueString, 16);
        }
    }

    public class ByteInputBox : NumericBox<byte>
    {
        public ByteInputBox()
        {
            MaxValue = byte.MaxValue;
            MinValue = byte.MinValue;
        }
        internal override byte Interop(byte value1, byte value2, bool isAdd)
        {
            return isAdd ? (byte)(value1 + value2) : (byte)(value1 - value2);
        }

        protected override byte ToTValue(string valueString)
        {
            if (HexMode == HexMode.Ten)
            {
                return Convert.ToByte(valueString);
            }
            return Convert.ToByte(valueString, 16);
        }
    }

    public class FloatInputBox : NumericBox<float>
    {
        public FloatInputBox()
        {
            MaxValue = float.MaxValue;
            MinValue = float.MinValue;
        }
        internal override float Interop(float value1, float value2, bool isAdd)
        {
            return isAdd ? (float)(value1 + value2) : (float)(value1 - value2);
        }

        protected override float ToTValue(string valueString)
        {
            return (float)Convert.ToSingle(valueString);
        }

        internal override string ToDisplayString()
        {
            if (string.IsNullOrWhiteSpace(StringFormat))
            {
                return Value.ToString();
            }
            return Value.ToString(StringFormat);
        }


        public static readonly DependencyProperty StringFormatProperty = PropertyAssist.PropertyRegister<FloatInputBox, string>(i => i.StringFormat);
        [Bindable(true), Category("StringFormat")]
        [Localizability(LocalizationCategory.None, Readability = Readability.Unreadable)]
        public string StringFormat
        {
            get => (string)GetValue(StringFormatProperty);
            set => SetValue(StringFormatProperty, value);
        }
    }

    public class DoubleInputBox : NumericBox<double>
    {
        public DoubleInputBox()
        {
            MaxValue = double.MaxValue;
            MinValue = double.MinValue;
        }
        internal override double Interop(double value1, double value2, bool isAdd)
        {
            return isAdd ? (double)(value1 + value2) : (double)(value1 - value2);
        }

        protected override double ToTValue(string valueString)
        {
            return (double)Convert.ToDouble(valueString);
        }
        internal override string ToDisplayString()
        {
            if (string.IsNullOrWhiteSpace(StringFormat))
            {
                return Value.ToString();
            }
            return Value.ToString(StringFormat);
        }


        public static readonly DependencyProperty StringFormatProperty = PropertyAssist.PropertyRegister<DoubleInputBox, string>(i => i.StringFormat);
        [Bindable(true), Category("StringFormat")]
        [Localizability(LocalizationCategory.None, Readability = Readability.Unreadable)]
        public string StringFormat
        {
            get => (string)GetValue(StringFormatProperty);
            set => SetValue(StringFormatProperty, value);
        }
    }

    public class DecimalInputBox : NumericBox<decimal>
    {
        public DecimalInputBox()
        {
            MaxValue = decimal.MaxValue;
            MinValue = decimal.MinValue;
        }
        internal override decimal Interop(decimal value1, decimal value2, bool isAdd)
        {
            return isAdd ? value1 + value2 : value1 - value2;
        }

        protected override decimal ToTValue(string valueString)
        {
            return Convert.ToDecimal(valueString);
        }
        internal override string ToDisplayString()
        {
            if (string.IsNullOrWhiteSpace(StringFormat))
            {
                return Value.ToString();
            }
            return Value.ToString(StringFormat);
        }


        public static readonly DependencyProperty StringFormatProperty = PropertyAssist.PropertyRegister<DecimalInputBox, string>(i => i.StringFormat);
        [Bindable(true), Category("StringFormat")]
        [Localizability(LocalizationCategory.None, Readability = Readability.Unreadable)]
        public string StringFormat
        {
            get => (string)GetValue(StringFormatProperty);
            set => SetValue(StringFormatProperty, value);
        }
    }
}
