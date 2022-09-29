



using Microsoft.Maui.Controls.Shapes;

using System.Diagnostics;

namespace Slight.Core
{
    public class PopupMessageBox : ContentView, IPopupMessage
    {

        [DebuggerBrowsable(DebuggerBrowsableState.Never)] private Border BackgroundContainer;
        [DebuggerBrowsable(DebuggerBrowsableState.Never)] private Label MessageBox;
        [DebuggerBrowsable(DebuggerBrowsableState.Never)] private Label TitleBox;
        [DebuggerBrowsable(DebuggerBrowsableState.Never)] private Border ButtonArea;
        [DebuggerBrowsable(DebuggerBrowsableState.Never)] private Button cancelButton;
        [DebuggerBrowsable(DebuggerBrowsableState.Never)] private Button confirmButton;
        internal PopupMessageBox()
        {
            ControlTemplate = new ControlTemplate(() =>
            {
                Border border = BackgroundContainer = new Border()
                {
                    Padding = new Thickness(0),
                    StrokeThickness = BorderThickness,
                    Stroke = BorderBrush,
                    BackgroundColor = BackgroundColor,
                    StrokeShape = new RoundRectangle() { CornerRadius = new CornerRadius(6) },
                };

                Grid grid = new()
                {
                    IsClippedToBounds = true,
                };
                grid.RowDefinitions.Add(new RowDefinition(new GridLength(60, GridUnitType.Absolute)));
                grid.RowDefinitions.Add(new RowDefinition(new GridLength(1, GridUnitType.Star)));
                grid.RowDefinitions.Add(new RowDefinition(new GridLength(80, GridUnitType.Absolute)));

                grid.Add(border);
                grid.SetRow(border, 0);
                grid.SetRowSpan(border, 3);

                grid.Add(TitleBox = new Label()
                {
                    Margin = new Thickness(20, 15, 0, 0),
                    FontSize = 20,
                    TextColor = TextColor
                });
                grid.SetRow(TitleBox, 0);

                ScrollView scrollView = new()
                {
                    Content = MessageBox = new Label()
                    {
                        Margin = new Thickness(20, 10, 20, 10),
                        TextColor = TextColor
                    },
                };
                grid.Add(scrollView);
                grid.SetRow(scrollView, 1);


                Grid grid2 = new()
                {
                    IsClippedToBounds = true,
                };
                grid.Add(grid2);
                grid.SetRow(grid2, 2);

                grid2.Add(ButtonArea = new Border()
                {
                    Margin = new Thickness(0),
                    StrokeShape = new RoundRectangle()
                    {
                        CornerRadius = new CornerRadius(0, 0, 6, 6),
                    },
                    StrokeThickness = 0,
                    Stroke = Brush.Transparent,
                    BackgroundColor = ButtonAreaColor,
                    Style = null,
                });

                StackLayout stack = new()
                {
                    Orientation = StackOrientation.Horizontal
                };
                grid2.Add(stack);

                stack.Add(cancelButton = new Button()
                {
                    Margin = new Thickness(0),
                    Padding = new Thickness(0, 0, 0, 2),
                    HeightRequest = 32,
                    HorizontalOptions = LayoutOptions.EndAndExpand,
                    Opacity = 0.8,
                    Text = NoText,
                    WidthRequest = 90,
                });

                stack.Add(confirmButton = new Button()
                {
                    Margin = new Thickness(10, 0),
                    Padding = new Thickness(0, 0, 0, 2),
                    HeightRequest = 32,
                    Text = YesText,
                    WidthRequest = 100,
                });

                confirmButton.Clicked += (s, e) => RequestClose?.Invoke(this, true);
                cancelButton.Clicked += (s, e) => RequestClose?.Invoke(this, false);

                return grid;
            });
        }

        public string Title
        {
            set => TitleBox.Text = value;
        }
        public string Message
        {
            set => MessageBox.Text = value;
        }

        public event EventHandler<PopupResultEventArgs> RequestClose;

        public void DisplayCancelVisual(bool isVisible)
        {
            if (cancelButton.IsVisible == isVisible)
            {
                return;
            }
            cancelButton.IsVisible = isVisible;
        }



        #region


        public static readonly BindableProperty ButtonAreaColorProperty =
           PropertyAssist.PropertyRegister<PopupMessageBox, Color>(i => i.ButtonAreaColor, Color.FromArgb("FFC8C8C8"), BindingMode.OneWay, (s, e) =>
           {
               Invoker.WhenNotNull(s.ButtonArea, i => i.BackgroundColor = e.NewValue);
           });

        public Color ButtonAreaColor
        {
            get => (Color)GetValue(ButtonAreaColorProperty);
            set => SetValue(ButtonAreaColorProperty, value);
        }

        public static readonly BindableProperty TextColorProperty =
            PropertyAssist.PropertyRegister<PopupMessageBox, Color>(i => i.TextColor, Colors.Black, BindingMode.OneWay, (s, e) =>
            {
                Invoker.WhenNotNull(s.MessageBox, i => i.TextColor = e.NewValue);
                Invoker.WhenNotNull(s.TitleBox, i => i.TextColor = e.NewValue);
            });

        public Color TextColor
        {
            get => (Color)GetValue(TextColorProperty);
            set => SetValue(TextColorProperty, value);
        }

        public static new readonly BindableProperty BackgroundColorProperty =
         PropertyAssist.PropertyRegister<PopupMessageBox, Color>(i => i.BackgroundColor, Colors.White, BindingMode.OneWay, (s, e) =>
         {
             Invoker.WhenNotNull(s.BackgroundContainer, i => i.BackgroundColor = e.NewValue);
         });

        public new Color BackgroundColor
        {
            get => (Color)GetValue(BackgroundColorProperty);
            set => SetValue(BackgroundColorProperty, value);
        }

        public static new readonly BindableProperty BackgroundProperty =
            PropertyAssist.PropertyRegister<PopupMessageBox, Brush>(i => i.Background, Brush.White, BindingMode.OneWay, (s, e) =>
            {
                Invoker.WhenNotNull(s.BackgroundContainer, i => i.Background = e.NewValue);
            });

        public new Brush Background
        {
            get => (Brush)GetValue(BackgroundProperty);
            set => SetValue(BackgroundProperty, value);
        }


        public static readonly BindableProperty NoTextProperty =
       PropertyAssist.PropertyRegister<PopupMessageBox, string>(i => i.NoText, "No", BindingMode.OneWay, (s, e) =>
       {
           Invoker.WhenNotNull(s.cancelButton, i => i.Text = e.NewValue);
       });

        public string NoText
        {
            get => (string)GetValue(NoTextProperty);
            set => SetValue(NoTextProperty, value);
        }


        public static readonly BindableProperty YesTextProperty =
       PropertyAssist.PropertyRegister<PopupMessageBox, string>(i => i.YesText, "Yes", BindingMode.OneWay, (s, e) =>
       {
           Invoker.WhenNotNull(s.confirmButton, i => i.Text = e.NewValue);
       });

        public string YesText
        {
            get => (string)GetValue(YesTextProperty);
            set => SetValue(YesTextProperty, value);
        }




        public static readonly BindableProperty BorderBrushProperty =
          PropertyAssist.PropertyRegister<PopupMessageBox, Brush>(i => i.BorderBrush, Brush.Gray, BindingMode.OneWay, (s, e) =>
          {
              Invoker.WhenNotNull(s.BackgroundContainer, i => i.Stroke = e.NewValue);
          });

        public Brush BorderBrush
        {
            get => (Brush)GetValue(BorderBrushProperty);
            set => SetValue(BorderBrushProperty, value);
        }

        public static readonly BindableProperty BorderThicknessProperty =
         PropertyAssist.PropertyRegister<PopupMessageBox, double>(i => i.BorderThickness, 0.5d, BindingMode.OneWay, (s, e) =>
         {
             Invoker.WhenNotNull(s.BackgroundContainer, i => i.StrokeThickness = e.NewValue);
         });

        public double BorderThickness
        {
            get => (double)GetValue(BorderThicknessProperty);
            set => SetValue(BorderThicknessProperty, value);
        }

        #endregion
    }
}
