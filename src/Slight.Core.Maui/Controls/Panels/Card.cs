



using Microsoft.Maui.Controls.Shapes;

using System.ComponentModel;
using System.Diagnostics;
using System.Numerics;
namespace Slight.Core
{
    [ContentProperty(nameof(Card.Content))]
    [DefaultProperty(nameof(Card.Content))]
    public class Card : ContentView
    {
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private Border border;

        public Card()
        {
            Shadow = new Shadow()
            {
                Brush = Brush.Black,
                Opacity = 0.8f,
                Radius = 10,
                Offset = new Point(0, 0),
            };

            ControlTemplate = new ControlTemplate(() =>
            {
                border = new Border
                {
                    Content = Content,
                    Background = Background,
                    BackgroundColor = BackgroundColor,
                    Stroke = BorderBrush,
                    StrokeThickness = BorderThickness,
                    StrokeShape = new RoundRectangle() { CornerRadius = CornerRadius }
                };

                return border;
            });



        }


        public static new readonly BindableProperty ContentProperty = PropertyAssist.PropertyRegister<Card, View>(i => i.Content, null, BindingMode.OneWay, (s, e) =>
        {
            Invoker.WhenNotNull(s.border, i => i.Content = e.NewValue);
        });

        [Bindable(true), Category("Content")]
        public new View Content
        {
            get => (View)GetValue(ContentProperty);
            set => SetValue(ContentProperty, value);
        }

        public static new readonly BindableProperty BackgroundProperty = PropertyAssist.PropertyRegister<Card, Brush>(i => i.Background, Brush.White, BindingMode.OneWay, (s, e) =>
        {
            Invoker.WhenNotNull(s.border, i => i.Background = e.NewValue);
        });

        [Bindable(true), Category("Background")]
        public new Brush Background
        {
            get => (Brush)GetValue(BackgroundProperty);
            set => SetValue(BackgroundProperty, value);
        }

        public static new readonly BindableProperty BackgroundColorProperty = PropertyAssist.PropertyRegister<Card, Color>(i => i.BackgroundColor, Colors.White, BindingMode.OneWay, (s, e) =>
        {
            Invoker.WhenNotNull(s.border, i => i.BackgroundColor = e.NewValue);
        });

        [Bindable(true), Category("Background")]
        public new Color BackgroundColor
        {
            get => (Color)GetValue(BackgroundColorProperty);
            set => SetValue(BackgroundColorProperty, value);
        }




        public static readonly BindableProperty BorderBrushProperty = PropertyAssist.PropertyRegister<Card, Brush>(i => i.BorderBrush, Brush.Transparent, BindingMode.OneWay, (s, e) =>
        {
            Invoker.WhenNotNull(s.border, i => i.Stroke = e.NewValue);
        });

        [Bindable(true), Category("Stroke")]
        public Brush BorderBrush
        {
            get => (Brush)GetValue(BorderBrushProperty);
            set => SetValue(BorderBrushProperty, value);
        }


        public static readonly BindableProperty BorderThicknessProperty = PropertyAssist.PropertyRegister<Card, double>(i => i.BorderThickness, 0d, BindingMode.OneWay, (s, e) =>
        {
            Invoker.WhenNotNull(s.border, i => i.StrokeThickness = e.NewValue);
        });

        [Bindable(true), Category("Stroke")]
        public double BorderThickness
        {
            get => (double)GetValue(BorderThicknessProperty);
            set => SetValue(BorderThicknessProperty, value);
        }


        public static readonly BindableProperty CornerRadiusProperty = PropertyAssist.PropertyRegister<Card, CornerRadius>(i => i.CornerRadius, new CornerRadius(0), BindingMode.OneWay, (s, e) =>
        {
            Invoker.WhenNotNull(s.border, i => i.StrokeShape = new RoundRectangle()
            {
                CornerRadius = e.NewValue
            });
        });

        [Bindable(true), Category("Border")]
        public CornerRadius CornerRadius
        {
            get => (CornerRadius)GetValue(CornerRadiusProperty);
            set => SetValue(CornerRadiusProperty, value);
        }


        public static readonly BindableProperty ShadowBrushProperty = PropertyAssist.PropertyRegister<Card, Brush>(i => i.ShadowBrush, Brush.Black, BindingMode.OneWay, (s, e) =>
        {
            Invoker.WhenNotNull(s.Shadow, i => i.Brush = e.NewValue);
        });

        [Bindable(true), Category("Shadow")]
        public Brush ShadowBrush
        {
            get => (Brush)GetValue(ShadowBrushProperty);
            set => SetValue(ShadowBrushProperty, value);
        }



        public static readonly BindableProperty ShadowOpacityProperty = PropertyAssist.PropertyRegister<Card, double>(i => i.ShadowOpacity, 0.8d, (s, e) =>
        {
            Invoker.WhenNotNull(s.Shadow, i => i.Opacity = (float)e.NewValue);
        });

        [Bindable(true), Category("Shadow")]
        public double ShadowOpacity
        {
            get => (double)GetValue(ShadowOpacityProperty);
            set => SetValue(ShadowOpacityProperty, value);
        }


        public static readonly BindableProperty ShadowRadiusProperty = PropertyAssist.PropertyRegister<Card, double>(i => i.ShadowRadius, 10d, (s, e) =>
        {
            Invoker.WhenNotNull(s.Shadow, i => i.Radius = (float)e.NewValue);
        });

        [Bindable(true), Category("Shadow")]
        public double ShadowRadius
        {
            get => (double)GetValue(ShadowRadiusProperty);
            set => SetValue(ShadowRadiusProperty, value);
        }

        public static readonly BindableProperty ShadowOffsetProperty = PropertyAssist.PropertyRegister<Card, Vector2>(i => i.ShadowOffset, new Vector2(0, 0), (s, e) =>
        {
            Invoker.WhenNotNull(s.Shadow, i => i.Offset = e.NewValue);
        });

        [Bindable(true), Category("Shadow")]
        public Vector2 ShadowOffset
        {
            get => (Vector2)GetValue(ShadowOffsetProperty);
            set => SetValue(ShadowOffsetProperty, value);
        }
    }
}
