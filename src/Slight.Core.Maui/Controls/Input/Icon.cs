

using Microsoft.Maui.Controls.Shapes;

using System.ComponentModel;
using System.Diagnostics;
using System.Globalization;
using System.Windows.Input;

using Path = Microsoft.Maui.Controls.Shapes.Path;
namespace Slight.Core
{
    public class Icon : ContentView
    {
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private static readonly Lazy<IDictionary<IconKind, string>> dataIndex = new(IconKindHelper.CreateIconMapper);

        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private Border border;

        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private Path path;

        public Icon()
        {
            ControlTemplate = new ControlTemplate(() =>
            {
                Grid grid = new();

                RoundRectangle rectangle;
                border = new Border
                {
                    Stroke = BorderBrush,
                    StrokeThickness = BorderThickness,
                    Background = Background,
                    BackgroundColor = BackgroundColor,
                    Content = grid,
                    StrokeShape = rectangle = new RoundRectangle()
                };
                rectangle.CornerRadius = CornerRadius;

                Button button = new()
                {
                    ZIndex = 2,
                    Background = Brush.Transparent,
                    BackgroundColor = Colors.Transparent,
                    CornerRadius = 0,
                    BorderWidth = 0,
                    BorderColor = Colors.Transparent
                };

                button.Clicked += HiIconClicked;

                path = new Path
                {
                    Margin = Padding,
                    ZIndex = 1,
                    Aspect = Stretch,
                    StrokeThickness = StrokeThickness,
                    Stroke = Stroke,
                    Fill = Fill,
                    Shadow = Shadow
                };

                if (dataIndex.Value.TryGetValue(Kind, out string data))
                {
                    path.Data = (Geometry)new PathGeometryConverter().ConvertFromString(data);
                }


                grid.Add(button);
                grid.Add(path);

                return border;
            });



        }

        private void HiIconClicked(object sender, EventArgs e)
        {
            if (IsClickable == false)
            {
                return;
            }

            Click?.Invoke(this, e);

            if (Command != null && Command.CanExecute(CommandParameter))
            {
                Command.Execute(CommandParameter);
            }
        }

        public static readonly BindableProperty KindProperty = PropertyAssist.PropertyRegister<Icon, IconKind>(i => i.Kind, IconKind.Abc, (s, e) =>
        {
            string data = null;
            dataIndex.Value?.TryGetValue(e.NewValue, out data);

            Invoker.WhenNotNull(s.path, i =>
            {
                i.Data = (Geometry)new PathGeometryConverter().ConvertFromString(data);
            });

        });

        [Bindable(true), Category("Kind")]
        public IconKind Kind
        {
            get => (IconKind)GetValue(KindProperty);
            set => SetValue(KindProperty, value);
        }

        public static new readonly BindableProperty PaddingProperty = PropertyAssist.PropertyRegister<Icon, Thickness>(i => i.Padding, new Thickness(5), BindingMode.OneWay, (s, e) =>
        {
            Invoker.WhenNotNull(s.path, i => i.Margin = e.NewValue);
        });

        [Bindable(true), Category("Padding")]
        public new Thickness Padding
        {
            get => (Thickness)GetValue(PaddingProperty);
            set => SetValue(PaddingProperty, value);
        }


        public static readonly BindableProperty StrokeProperty = PropertyAssist.PropertyRegister<Icon, Brush>(i => i.Stroke, Brush.Black, BindingMode.OneWay, (s, e) =>
        {
            Invoker.WhenNotNull(s.path, i => i.Stroke = e.NewValue);
        });

        [Bindable(true), Category("Stroke")]
        public Brush Stroke
        {
            get => (Brush)GetValue(StrokeProperty);
            set => SetValue(StrokeProperty, value);
        }


        public static readonly BindableProperty StrokeThicknessProperty = PropertyAssist.PropertyRegister<Icon, double>(i => i.StrokeThickness, 1d, BindingMode.OneWay, (s, e) =>
        {
            Invoker.WhenNotNull(s.path, i => i.StrokeThickness = e.NewValue);
        });

        [Bindable(true), Category("Stroke")]
        public double StrokeThickness
        {
            get => (double)GetValue(StrokeThicknessProperty);
            set => SetValue(StrokeThicknessProperty, value);
        }

        public static readonly BindableProperty FillProperty = PropertyAssist.PropertyRegister<Icon, Brush>(i => i.Fill, Brush.Black, BindingMode.OneWay, (s, e) =>
        {
            Invoker.WhenNotNull(s.path, i => i.Fill = e.NewValue);
        });

        [Bindable(true), Category("Stroke")]
        public Brush Fill
        {
            get => (Brush)GetValue(FillProperty);
            set => SetValue(FillProperty, value);
        }


        public static readonly BindableProperty StretchProperty = PropertyAssist.PropertyRegister<Icon, Stretch>(i => i.Stretch, Stretch.Fill, BindingMode.OneWay, (s, e) =>
        {
            Invoker.WhenNotNull(s.path, i => i.Aspect = e.NewValue);
        });

        [Bindable(true), Category("Stretch")]
        public Stretch Stretch
        {
            get => (Stretch)GetValue(StretchProperty);
            set => SetValue(StretchProperty, value);
        }


        public static readonly BindableProperty StrokeDashOffsetProperty = PropertyAssist.PropertyRegister<Icon, double>(i => i.StrokeDashOffset, 0d, BindingMode.OneWay, (s, e) =>
        {
            Invoker.WhenNotNull(s.path, i => i.StrokeDashOffset = e.NewValue);
        });

        [Bindable(true), Category("DashOffset")]
        public double StrokeDashOffset
        {
            get => (double)GetValue(StrokeDashOffsetProperty);
            set => SetValue(StrokeDashOffsetProperty, value);
        }









        public static readonly BindableProperty CornerRadiusProperty = PropertyAssist.PropertyRegister<Icon, CornerRadius>(i => i.CornerRadius, new CornerRadius(0), BindingMode.OneWay, (s, e) =>
        {
            Invoker.WhenNotNull(s.border, i => i.StrokeShape = new RoundRectangle()
            {
                CornerRadius = e.NewValue,
            });
        });

        [Bindable(true), Category("Border")]
        public CornerRadius CornerRadius
        {
            get => (CornerRadius)GetValue(CornerRadiusProperty);
            set => SetValue(CornerRadiusProperty, value);
        }

        public static new readonly BindableProperty BackgroundProperty = PropertyAssist.PropertyRegister<Icon, Brush>(i => i.Background, Brush.Transparent, BindingMode.OneWay, (s, e) =>
        {
            Invoker.WhenNotNull(s.border, i => i.Background = e.NewValue);
        });

        [Bindable(true), Category("Border")]
        public new Brush Background
        {
            get => (Brush)GetValue(BackgroundProperty);
            set => SetValue(BackgroundProperty, value);
        }

        public static new readonly BindableProperty BackgroundColorProperty = PropertyAssist.PropertyRegister<Icon, Color>(i => i.BackgroundColor, Colors.Transparent, BindingMode.OneWay, (s, e) =>
        {
            Invoker.WhenNotNull(s.border, i => i.BackgroundColor = e.NewValue);
        });

        [Bindable(true), Category("Border")]
        public new Color BackgroundColor
        {
            get => (Color)GetValue(BackgroundColorProperty);
            set => SetValue(BackgroundColorProperty, value);
        }

        public static readonly BindableProperty BorderColorProperty = PropertyAssist.PropertyRegister<Icon, Brush>(i => i.BorderBrush, Brush.Transparent, BindingMode.OneWay, (s, e) =>
        {
            Invoker.WhenNotNull(s.border, i => i.Stroke = e.NewValue);
        });

        [Bindable(true), Category("Border")]
        public Brush BorderBrush
        {
            get => (Brush)GetValue(BorderColorProperty);
            set => SetValue(BorderColorProperty, value);
        }

        public static readonly BindableProperty BorderThicknessProperty = PropertyAssist.PropertyRegister<Icon, double>(i => i.BorderThickness, 0d, BindingMode.OneWay, (s, e) =>
        {
            Invoker.WhenNotNull(s.border, i => i.StrokeThickness = e.NewValue);
        });

        [Bindable(true), Category("Border")]
        public double BorderThickness
        {
            get => (double)GetValue(BorderThicknessProperty);
            set => SetValue(BorderThicknessProperty, value);
        }

        public static readonly BindableProperty IsClickableProperty = PropertyAssist.PropertyRegister<Icon, bool>(i => i.IsClickable, true, BindingMode.OneWay);

        [Bindable(true), Category("Click")]
        public bool IsClickable
        {
            get => (bool)GetValue(IsClickableProperty);
            set => SetValue(IsClickableProperty, value);
        }


        public static readonly BindableProperty CommandProperty = PropertyAssist.PropertyRegister<Icon, ICommand>(p => p.Command, null);

        [Bindable(true), Category("Click")]
        public ICommand Command
        {
            get => (ICommand)GetValue(CommandProperty);
            set => SetValue(CommandProperty, value);
        }

        public static readonly BindableProperty CommandParameterProperty = PropertyAssist.PropertyRegister<Icon, object>(p => p.CommandParameter, null);

        [Bindable(true), Category("Click")]
        public object CommandParameter
        {
            get => GetValue(CommandParameterProperty);
            set => SetValue(CommandParameterProperty, value);
        }

        public event EventHandler Click;


        private class DataConverter : IValueConverter
        {
            private static readonly PathGeometryConverter converter = new();

            public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
            {
                if (value is string data)
                {
                    object vv = converter.ConvertFromString(data);
                    return vv;
                }

                return null;
            }

            public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
            {
                if (value is PathGeometry pathGeometry)
                {
                    string s = pathGeometry.ToString();
                    return s;
                }
                return null;
            }
        }
    }
}
