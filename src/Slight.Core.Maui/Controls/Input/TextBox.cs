
using Microsoft.Maui.Controls.Shapes;

using System.Diagnostics;

namespace Slight.Core
{

    public class TextBox : ContentView
    {
        [DebuggerBrowsable(DebuggerBrowsableState.Never)] private Border backgoundVisual;
        [DebuggerBrowsable(DebuggerBrowsableState.Never)] private Entry inputVisual;
        public TextBox()
        {
            ControlTemplate = new ControlTemplate(() =>
            {
                Border border = backgoundVisual = new Border();
                border.StrokeShape = new RoundRectangle() { CornerRadius = CornerRadius };
                border.StrokeThickness = BorderThickness;
                border.Background = Background;
                border.Stroke = BorderBrush;
                border.Background = Background;
                border.BackgroundColor = BackgroundColor;

                inputVisual = new Entry();
                if (string.IsNullOrWhiteSpace(FontFamily) == false)
                {
                    inputVisual.FontFamily = FontFamily;
                }
                inputVisual.MaxLength = MaxLength;
                inputVisual.Style = null;
                inputVisual.Background = Brush.Transparent;
                inputVisual.BackgroundColor = Colors.Transparent;
                inputVisual.Text = Text;
                inputVisual.VerticalTextAlignment = VerticalTextAlignment;
                inputVisual.HorizontalTextAlignment = HorizontalTextAlignment;
                inputVisual.IsPassword = IsPasswordBox;
                inputVisual.FontSize = FontSize;
                inputVisual.CursorPosition = CursorPosition;
                inputVisual.Placeholder = Placeholder;
                inputVisual.IsReadOnly = IsReadOnly;
                inputVisual.TextColor = TextColor;
                inputVisual.PlaceholderColor = PlaceholderColor;
                inputVisual.VerticalOptions = LayoutOptions.Fill;
                inputVisual.HorizontalOptions = LayoutOptions.Fill;
                inputVisual.Keyboard = Keyboard;
                inputVisual.TextTransform = TextTransform;
                inputVisual.CharacterSpacing = CharacterSpacing;
                inputVisual.Margin = Padding;







                border.Content = inputVisual;

                Grid grid = new()
                {
                    IsClippedToBounds = true
                };
                grid.Add(border);

                return grid;

            });

            inputVisual.TextChanged += (s, e) => TextChanged?.Invoke(this, e);
            inputVisual.SetBinding(Entry.TextProperty, new Binding($"{nameof(Text)}", BindingMode.TwoWay, null, null, null, this));

        }

        public static readonly BindableProperty CornerRadiusProperty =
            PropertyAssist.PropertyRegister<TextBox, CornerRadius>(i => i.CornerRadius, new CornerRadius(0), BindingMode.OneWay, (s, e) =>
            {
                Invoker.WhenNotNull(s.backgoundVisual, i => i.StrokeShape = new RoundRectangle()
                {
                    CornerRadius = e.NewValue,
                });
            });

        public CornerRadius CornerRadius
        {
            get => (CornerRadius)GetValue(CornerRadiusProperty);
            set => SetValue(CornerRadiusProperty, value);
        }


        public static new readonly BindableProperty BackgroundProperty =
           PropertyAssist.PropertyRegister<TextBox, Brush>(i => i.Background, Brush.White, BindingMode.OneWay, (s, e) =>
           {
               Invoker.WhenNotNull(s.backgoundVisual, i => i.Background = e.NewValue);
           });

        public new Brush Background
        {
            get => (Brush)GetValue(BackgroundProperty);
            set => SetValue(BackgroundProperty, value);
        }

        public static readonly BindableProperty BorderBrushProperty =
         PropertyAssist.PropertyRegister<TextBox, Brush>(i => i.BorderBrush, Brush.LightGray, BindingMode.OneWay, (s, e) =>
         {
             Invoker.WhenNotNull(s.backgoundVisual, i => i.Stroke = e.NewValue);
         });

        public Brush BorderBrush
        {
            get => (Brush)GetValue(BorderBrushProperty);
            set => SetValue(BorderBrushProperty, value);
        }

        public static new readonly BindableProperty BackgroundColorProperty =
         PropertyAssist.PropertyRegister<TextBox, Color>(i => i.BackgroundColor, Colors.White, BindingMode.OneWay, (s, e) =>
         {
             Invoker.WhenNotNull(s.backgoundVisual, i => i.BackgroundColor = e.NewValue);
         });

        public new Color BackgroundColor
        {
            get => (Color)GetValue(BackgroundColorProperty);
            set => SetValue(BackgroundColorProperty, value);
        }

        public static readonly BindableProperty BorderThicknessProperty =
         PropertyAssist.PropertyRegister<TextBox, double>(i => i.BorderThickness, 1d, BindingMode.OneWay, (s, e) =>
         {
             Invoker.WhenNotNull(s.backgoundVisual, i => i.StrokeThickness = e.NewValue);
         });

        public double BorderThickness
        {
            get => (double)GetValue(BorderThicknessProperty);
            set => SetValue(BorderThicknessProperty, value);
        }




        public static readonly BindableProperty TextProperty =
         PropertyAssist.PropertyRegister<TextBox, string>(i => i.Text, null, BindingMode.TwoWay, (s, e) =>
         {
             Invoker.WhenNotNull(s.inputVisual, i => i.Text = e.NewValue);
         });

        public string Text
        {
            get => (string)GetValue(TextProperty);
            set => SetValue(TextProperty, value);
        }

        public static readonly BindableProperty FontFamilyProperty =
        PropertyAssist.PropertyRegister<TextBox, string>(i => i.FontFamily, null, BindingMode.OneWay, (s, e) =>
        {
            Invoker.WhenNotNull(s.inputVisual, i => i.FontFamily = e.NewValue);
        });

        public string FontFamily
        {
            get => (string)GetValue(FontFamilyProperty);
            set => SetValue(FontFamilyProperty, value);
        }


        public static readonly BindableProperty IsPasswordBoxProperty =
            PropertyAssist.PropertyRegister<TextBox, bool>(i => i.IsPasswordBox, false, BindingMode.OneWay, (s, e) =>
            {
                Invoker.WhenNotNull(s.inputVisual, i => i.IsPassword = e.NewValue);
            });

        public bool IsPasswordBox
        {
            get => (bool)GetValue(IsPasswordBoxProperty);
            set => SetValue(IsPasswordBoxProperty, value);
        }


        public static readonly BindableProperty VerticalTextAlignmentProperty =
            PropertyAssist.PropertyRegister<TextBox, TextAlignment>(i => i.VerticalTextAlignment, TextAlignment.Start, BindingMode.OneWay, (s, e) =>
            {
                Invoker.WhenNotNull(s.inputVisual, i => i.VerticalTextAlignment = e.NewValue);
            });

        public TextAlignment VerticalTextAlignment
        {
            get => (TextAlignment)GetValue(VerticalTextAlignmentProperty);
            set => SetValue(VerticalTextAlignmentProperty, value);
        }


        public static readonly BindableProperty HorizontalTextAlignmentProperty =
            PropertyAssist.PropertyRegister<TextBox, TextAlignment>(i => i.HorizontalTextAlignment, TextAlignment.Start, BindingMode.OneWay, (s, e) =>
            {
                Invoker.WhenNotNull(s.inputVisual, i => i.HorizontalTextAlignment = e.NewValue);
            });

        public TextAlignment HorizontalTextAlignment
        {
            get => (TextAlignment)GetValue(HorizontalTextAlignmentProperty);
            set => SetValue(HorizontalTextAlignmentProperty, value);
        }


        public static readonly BindableProperty FontSizeProperty =
            PropertyAssist.PropertyRegister<TextBox, double>(i => i.FontSize, 0, BindingMode.OneWay, (s, e) =>
            {
                Invoker.WhenNotNull(s.inputVisual, i => i.FontSize = e.NewValue);
            });

        public double FontSize
        {
            get => (double)GetValue(FontSizeProperty);
            set => SetValue(FontSizeProperty, value);
        }

        public static readonly BindableProperty CursorPositionProperty =
         PropertyAssist.PropertyRegister<TextBox, int>(i => i.CursorPosition, 0, BindingMode.OneWay, (s, e) =>
         {
             Invoker.WhenNotNull(s.inputVisual, i => i.CursorPosition = e.NewValue);
         });

        public int CursorPosition
        {
            get => (int)GetValue(CursorPositionProperty);
            set => SetValue(CursorPositionProperty, value);
        }



        public static readonly BindableProperty PlaceholderProperty =
            PropertyAssist.PropertyRegister<TextBox, string>(i => i.Placeholder, "", BindingMode.OneWay, (s, e) =>
            {
                Invoker.WhenNotNull(s.inputVisual, i => i.Placeholder = e.NewValue);
            });

        public string Placeholder
        {
            get => (string)GetValue(PlaceholderProperty);
            set => SetValue(PlaceholderProperty, value);
        }



        public static readonly BindableProperty IsReadOnlyProperty =
          PropertyAssist.PropertyRegister<TextBox, bool>(i => i.IsReadOnly, false, BindingMode.OneWay, (s, e) =>
          {
              Invoker.WhenNotNull(s.inputVisual, i => i.IsReadOnly = e.NewValue);
          });

        public bool IsReadOnly
        {
            get => (bool)GetValue(IsReadOnlyProperty);
            set => SetValue(IsReadOnlyProperty, value);
        }



        public static readonly BindableProperty TextColorProperty =
         PropertyAssist.PropertyRegister<TextBox, Color>(i => i.TextColor, Colors.Black, BindingMode.OneWay, (s, e) =>
         {
             Invoker.WhenNotNull(s.inputVisual, i => i.TextColor = e.NewValue);
         });

        public Color TextColor
        {
            get => (Color)GetValue(TextColorProperty);
            set => SetValue(TextColorProperty, value);
        }


        public static readonly BindableProperty PlaceholderColorProperty =
         PropertyAssist.PropertyRegister<TextBox, Color>(i => i.PlaceholderColor, Colors.Gray, BindingMode.OneWay, (s, e) =>
         {
             Invoker.WhenNotNull(s.inputVisual, i => i.PlaceholderColor = e.NewValue);
         });

        public Color PlaceholderColor
        {
            get => (Color)GetValue(PlaceholderColorProperty);
            set => SetValue(PlaceholderColorProperty, value);
        }



        public static readonly BindableProperty MaxLengthProperty =
            PropertyAssist.PropertyRegister<TextBox, int>(i => i.MaxLength, 0xffff, BindingMode.OneWay, (s, e) =>
            {
                Invoker.WhenNotNull(s.inputVisual, i => i.MaxLength = e.NewValue);
            });

        public int MaxLength
        {
            get => (int)GetValue(MaxLengthProperty);
            set => SetValue(MaxLengthProperty, value);
        }


        public static readonly BindableProperty KeyboardProperty =
         PropertyAssist.PropertyRegister<TextBox, Keyboard>(i => i.Keyboard, Keyboard.Default, BindingMode.OneWay, (s, e) =>
         {
             Invoker.WhenNotNull(s.inputVisual, i => i.Keyboard = e.NewValue);
         });

        public Keyboard Keyboard
        {
            get => (Keyboard)GetValue(KeyboardProperty);
            set => SetValue(KeyboardProperty, value);
        }

        public static readonly BindableProperty TextTransformProperty =
         PropertyAssist.PropertyRegister<TextBox, TextTransform>(i => i.TextTransform, TextTransform.Default, BindingMode.OneWay, (s, e) =>
         {
             Invoker.WhenNotNull(s.inputVisual, i => i.TextTransform = e.NewValue);
         });

        public TextTransform TextTransform
        {
            get => (TextTransform)GetValue(TextTransformProperty);
            set => SetValue(TextTransformProperty, value);
        }


        public static readonly BindableProperty CharacterSpacingProperty =
            PropertyAssist.PropertyRegister<TextBox, double>(i => i.CharacterSpacing, default, BindingMode.OneWay, (s, e) =>
            {
                Invoker.WhenNotNull(s.inputVisual, i => i.CharacterSpacing = e.NewValue);
            });

        public double CharacterSpacing
        {
            get => (double)GetValue(CharacterSpacingProperty);
            set => SetValue(CharacterSpacingProperty, value);
        }



        public static new readonly BindableProperty PaddingProperty =
            PropertyAssist.PropertyRegister<TextBox, Thickness>(i => i.CharacterSpacing, new Thickness(0, -2), BindingMode.OneWay, (s, e) =>
            {
                Invoker.WhenNotNull(s.inputVisual, i => i.Margin = e.NewValue);
            });

        public new Thickness Padding
        {
            get => (Thickness)GetValue(PaddingProperty);
            set => SetValue(PaddingProperty, value);
        }

        public event EventHandler<TextChangedEventArgs> TextChanged;



    }
}
