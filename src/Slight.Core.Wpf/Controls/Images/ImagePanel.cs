using System;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace Slight.Core
{
    public class ImagePanel : Canvas
    {
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private Point rightMousePos, leftStartMousePos, leftEndMousePos;
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private bool initialized, isLeftMouseDown, isRightMouseDown;
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private Line line1, line2, line3, line4;
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private EventState EventStatus = EventState.None;
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private Image image;
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private Grid grid;
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private ScaleTransform scaleTransform;
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private TranslateTransform translateTransform;
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private RotateTransform rotateTransform;
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private Action<System.Drawing.Rectangle> rectangleSelectionCallback;

        public static DependencyProperty ImageSourceProperty = PropertyAssist.PropertyRegister<ImagePanel, ImageSource>(i => i.ImageSource, null, (s, e) =>
        {
            if (s is null || s.image is null)
            {
                return;
            }
            s.image.Source = e.NewValue;
        });


        [Bindable(true)]
        [Category("ImageSource")]
        [Localizability(LocalizationCategory.None, Readability = Readability.Unreadable)]
        [EditorBrowsable(EditorBrowsableState.Never)]
        public ImageSource ImageSource
        {
            get => GetValue(ImageSourceProperty) as ImageSource;
            set => SetValue(ImageSourceProperty, value);
        }


        public ImagePanel()
        {
            Initialize();
        }


        private void Initialize()
        {
            if (initialized)
            {
                return;
            }
            initialized = true;
            Style = null;
            TransformGroup group = new();
            group.Children.Add(scaleTransform = new ScaleTransform());
            group.Children.Add(translateTransform = new TranslateTransform());
            group.Children.Add(rotateTransform = new RotateTransform());
            RenderTransform = group;


            MouseDown += Image_MouseDown;
            MouseUp += Image_MouseUp;
            MouseMove += Image_MouseMove;
            MouseWheel += Image_MouseWheel;

            image = new Image()
            {
                Source = ImageSource,
                Stretch = Stretch.UniformToFill,
                Style = null,

            };
            grid = new Grid()
            {
                Style = null,
            };


            Children.Add(image);
            Children.Add(grid);

            grid.Children.Add(line1 = Build());
            grid.Children.Add(line2 = Build());
            grid.Children.Add(line3 = Build());
            grid.Children.Add(line4 = Build());

            Line Build()
            {
                Line l = new()
                {
                    Stroke = Brushes.Red,
                    StrokeThickness = 1,
                    Style = null,
                };
                return l;
            }
        }

        [EditorBrowsable(EditorBrowsableState.Never)]
        public void AddClipHandles(Action<System.Drawing.Rectangle> rectangleSelectionCallback)
        {
            this.rectangleSelectionCallback = rectangleSelectionCallback;
        }

        private void Image_MouseDown(object sender, MouseButtonEventArgs e)
        {
            EventStatus = EventState.MouseDown;
            if (e.ChangedButton == MouseButton.Right)
            {
                isRightMouseDown = true;
                rightMousePos = e.GetPosition((IInputElement)Parent);
                CaptureMouse();
                return;
            }

            if (e.ChangedButton == MouseButton.Left)
            {
                isLeftMouseDown = true;
                leftStartMousePos = e.GetPosition(image);

                CaptureMouse();
                return;
            }


            if (EventStatus == EventState.MouseDown)
            {

            }
        }


        private void Image_MouseMove(object sender, MouseEventArgs e)
        {
            EventStatus = EventState.MouseMove;

            if (isRightMouseDown)
            {
                Point position = e.GetPosition((IInputElement)Parent);
                translateTransform.X -= rightMousePos.X - position.X;
                translateTransform.Y -= rightMousePos.Y - position.Y;
                rightMousePos = position;
                return;
            }

            if (isLeftMouseDown)
            {
                Point position = leftEndMousePos = e.GetPosition(image);

                UpdateBox(leftStartMousePos, position);
            }
        }

        private void Image_MouseUp(object sender, MouseButtonEventArgs e)
        {
            EventStatus = EventState.MouseUp;
            isRightMouseDown = false;

            if (isLeftMouseDown && rectangleSelectionCallback != null)
            {
                leftEndMousePos = e.GetPosition(image);
                int minX = (int)Math.Min(leftStartMousePos.X, leftEndMousePos.X);
                int minY = (int)Math.Min(leftStartMousePos.Y, leftEndMousePos.Y);
                int width = (int)Math.Abs(leftStartMousePos.X - leftEndMousePos.X);
                int height = (int)Math.Abs(leftStartMousePos.Y - leftEndMousePos.Y);

                if (width > 1 && height > 1)
                {
                    System.Drawing.Rectangle r = new(minX, minY, width, height);

                    rectangleSelectionCallback.Invoke(r);
                }
            }

            isLeftMouseDown = false;
            ReleaseMouseCapture();

        }

        private void Image_MouseWheel(object sender, MouseWheelEventArgs e)
        {
            EventStatus = EventState.MouseWheel;
            double delta = e.Delta * 0.001;

            Point previousPoint = e.GetPosition(image);

            if (scaleTransform.ScaleX + delta < 0.005)
            {
                return;
            }

            scaleTransform.ScaleX += delta;
            scaleTransform.ScaleY += delta;


            translateTransform.X += -1 * previousPoint.X * delta;
            translateTransform.Y += -1 * previousPoint.Y * delta;

        }


        public void ResetTransform()
        {

            this?.Dispatcher.InvokeAsync(new Action(() =>
            {
                scaleTransform.CenterY = scaleTransform.CenterY = 0;
                scaleTransform.ScaleX = scaleTransform.ScaleY = 1;
                translateTransform.X = translateTransform.Y = 0;
                rotateTransform.CenterX = rotateTransform.CenterY = rotateTransform.Angle = 0;
                UpdateBox(new Point(), new Point());
                UpdateLayout();
            }), System.Windows.Threading.DispatcherPriority.Background);

        }

        [EditorBrowsable(EditorBrowsableState.Never)]
        public void SetImageSource(string path)
        {
            if (string.IsNullOrEmpty(path))
            {
                UpdateBox(new Point(), new Point());
                image.Source = null;
                return;
            }

            BitmapImage imageSource = new();
            using FileStream stream = File.OpenRead(path);
            imageSource.BeginInit();
            imageSource.CacheOption = BitmapCacheOption.OnLoad;
            imageSource.StreamSource = stream;
            imageSource.EndInit();

            image.Width = (int)imageSource.Width;
            image.Height = (int)imageSource.Height;
            image.Source = imageSource;
        }

        [EditorBrowsable(EditorBrowsableState.Never)]
        public void SetImageSource(ImageSource imageSource)
        {

            image.Source = imageSource;
            if (imageSource is null)
            {
                UpdateBox(new Point(), new Point());
                return;
            }
            image.Width = (int)imageSource.Width;
            image.Height = (int)imageSource.Height;

        }


        public void UpdateBox(Rect rect)
        {
            UpdateBox(rect.TopLeft, rect.BottomRight);
        }


        private void UpdateBox(Point sp, Point ep)
        {
            line1.X1 = sp.X;
            line1.Y1 = sp.Y;
            line1.X2 = sp.X;
            line1.Y2 = ep.Y;

            line2.X1 = sp.X;
            line2.Y1 = sp.Y;
            line2.X2 = ep.X;
            line2.Y2 = sp.Y;

            line3.X1 = ep.X;
            line3.Y1 = sp.Y;
            line3.X2 = ep.X;
            line3.Y2 = ep.Y;


            line4.X1 = sp.X;
            line4.Y1 = ep.Y;
            line4.X2 = ep.X;
            line4.Y2 = ep.Y;
        }


        public enum EventState
        {
            MouseDown,
            MouseUp,
            MouseMove,
            MouseWheel,
            None
        }
    }
}
