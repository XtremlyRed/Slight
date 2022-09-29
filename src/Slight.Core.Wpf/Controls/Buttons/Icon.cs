using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;

using Brush = System.Windows.Media.Brush;
using Brushes = System.Windows.Media.Brushes;
using Control = System.Windows.Controls.Control;

namespace Slight.Core
{
    public class Icon : Control, ICommandSource
    {
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private static readonly Lazy<IDictionary<IconKind, string>> _dataIndex = new(IconKindHelper.CreateIconMapper);
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private readonly Storyboard storyboard = new();
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private bool isClickDown;

        static Icon()
        {
            PropertyAssist.DefaultStyle<Icon>(DefaultStyleKeyProperty);
        }


        public static readonly DependencyProperty KindProperty = PropertyAssist.PropertyRegister<Icon, IconKind>(i => i.Kind, IconKind.File, (s, e) => s.UpdateData());



        [Bindable(true), Category("Kind")]
        [Localizability(LocalizationCategory.None, Readability = Readability.Unreadable)]
        public IconKind Kind
        {
            get => (IconKind)GetValue(KindProperty);
            set => SetValue(KindProperty, value);
        }

        public static readonly DependencyProperty DataProperty = PropertyAssist.PropertyRegister<Icon, string>(i => i.Data, null/*, (s, e) => s.UpdateData()*/);


        [Bindable(true), Category("Kind")]
        [Localizability(LocalizationCategory.None, Readability = Readability.Unreadable)]
        [TypeConverter(typeof(GeometryConverter))]
        internal string Data
        {
            get => (string)GetValue(DataProperty);
            private set => SetValue(DataProperty, value);
        }

        public override void OnApplyTemplate()
        {
            base.OnApplyTemplate();
            storyboard.Children.Clear();

            const int totalTime = 300;
            SineEase ease = new()
            {
                EasingMode = EasingMode.EaseIn,
            };
            Border grid = Template.FindName("PATH_grid", this) as Border;

            storyboard.ObjectKeyFrameAnimationBuilder()
                .AddKeyFrame(Visibility.Visible, 0)
                .SetTarget(grid)
                .SetTargetProperty(new PropertyPath(Grid.VisibilityProperty))
                .Build();
            storyboard.ObjectKeyFrameAnimationBuilder()
                .AddKeyFrame(Visibility.Collapsed, totalTime)
                .SetTarget(grid)
                .SetTargetProperty(new PropertyPath(Grid.VisibilityProperty))
                .Build();

            storyboard.DoubleAnimationBuilder()
                .FromTo(1, 0, totalTime)
                .EasingFunction(ease)
                .SetTarget(grid)
                .SetTargetProperty(new PropertyPath(Grid.OpacityProperty));

            storyboard.DoubleAnimationBuilder()
                .FromTo(1, 2, totalTime)
                .EasingFunction(ease)
                .SetTarget(grid)
                .SetTargetProperty(new PropertyPath("(Grid.RenderTransform).(ScaleTransform.ScaleX)"));


            storyboard.DoubleAnimationBuilder()
                .FromTo(1, 2, totalTime)
                .EasingFunction(ease)
                .SetTarget(grid)
                .SetTargetProperty(new PropertyPath("(Grid.RenderTransform).(ScaleTransform.ScaleY)"));

            UpdateData();
        }

        private void UpdateData()
        {
            string data = null;
            _dataIndex.Value?.TryGetValue(Kind, out data);
            Data = data;
        }

        public static readonly DependencyProperty FeedbackForegroundProperty = PropertyAssist.PropertyRegister<Icon, Brush>(p => p.FeedbackForeground, Brushes.LightGray);



        [Bindable(true), Category("Feedback")]
        [Localizability(LocalizationCategory.None, Readability = Readability.Unreadable)]
        public Brush FeedbackForeground
        {
            get => (Brush)GetValue(FeedbackForegroundProperty);
            set => SetValue(FeedbackForegroundProperty, value);
        }

        public static readonly DependencyProperty FeedbackBackgroundProperty = PropertyAssist.PropertyRegister<Icon, Brush>(p => p.FeedbackBackground, Brushes.LightGray);

        [Bindable(true), Category("Feedback")]
        [Localizability(LocalizationCategory.None, Readability = Readability.Unreadable)]
        public Brush FeedbackBackground
        {
            get => (Brush)GetValue(FeedbackBackgroundProperty);
            set => SetValue(FeedbackBackgroundProperty, value);
        }

        #region Click


        private void Icon_MouseUp(object sender, MouseButtonEventArgs e)
        {
            if (isClickDown == false)
            {
                return;
            }

            try
            {
                if (EnableClickAnimation)
                {
                    storyboard.Begin();
                }

                Command.TryExecute(CommandParameter, CommandTarget);

                RaiseEvent(new RoutedEventArgs(ClickEvent, this));
            }
            finally
            {
                isClickDown = false;
            }
        }
        private void Icon_MouseDown(object sender, MouseButtonEventArgs e)
        {
            isClickDown = true;
        }

        public static readonly DependencyProperty CommandProperty = PropertyAssist.PropertyRegister<Icon, ICommand>(p => p.Command, null);

        [Bindable(true), Category("Command")]
        [Localizability(LocalizationCategory.None, Readability = Readability.Unreadable)]
        public ICommand Command
        {
            get => (ICommand)GetValue(CommandProperty);
            set => SetValue(CommandProperty, value);
        }

        public static readonly DependencyProperty CommandParameterProperty = PropertyAssist.PropertyRegister<Icon, object>(p => p.CommandParameter, null);

        [Bindable(true), Category("Command")]
        [Localizability(LocalizationCategory.None, Readability = Readability.Unreadable)]
        public object CommandParameter
        {
            get => GetValue(CommandParameterProperty);
            set => SetValue(CommandParameterProperty, value);
        }



        public static readonly DependencyProperty CommandTargetProperty = PropertyAssist.PropertyRegister<Icon, IInputElement>(p => p.CommandTarget, null);

        [Bindable(true), Category("Command")]
        [Localizability(LocalizationCategory.None, Readability = Readability.Unreadable)]
        public IInputElement CommandTarget
        {
            get => GetValue(CommandTargetProperty) as IInputElement;
            set => SetValue(CommandTargetProperty, value);
        }

        public static readonly DependencyProperty IsClickableProperty = PropertyAssist.PropertyRegister<Icon, bool>(p => p.IsClickable, false, (s, e) =>
        {
            if (e.NewValue == false)
            {
                s.MouseDown -= s.Icon_MouseDown;
                s.MouseUp -= s.Icon_MouseUp;
                return;
            }
            s.MouseUp += s.Icon_MouseUp;
            s.MouseDown += s.Icon_MouseDown;
        });



        [Bindable(true), Category("Click")]
        [Localizability(LocalizationCategory.None, Readability = Readability.Unreadable)]
        public bool IsClickable
        {
            get => (bool)GetValue(IsClickableProperty);
            set => SetValue(IsClickableProperty, value);
        }

        public static readonly DependencyProperty EnableClickAnimationProperty = PropertyAssist.PropertyRegister<Icon, bool>(p => p.EnableClickAnimation, true);


        [Bindable(true), Category("Click")]
        [Localizability(LocalizationCategory.None, Readability = Readability.Unreadable)]
        public bool EnableClickAnimation
        {
            get => (bool)GetValue(EnableClickAnimationProperty);
            set => SetValue(EnableClickAnimationProperty, value);
        }


        public static readonly RoutedEvent ClickEvent =
         EventManager.RegisterRoutedEvent(
            nameof(Click),
             RoutingStrategy.Bubble,
             typeof(EventHandler),
             typeof(Icon));

        /// <summary>
        /// Raised when the popup is opened.
        /// </summary>
        public event RoutedEventHandler Click
        {
            add => AddHandler(ClickEvent, value);
            remove => RemoveHandler(ClickEvent, value);
        }



        public static readonly DependencyProperty CornerRadiusProperty = PropertyAssist.PropertyRegister<Icon, CornerRadius>(p => p.CornerRadius, new CornerRadius(5), FrameworkPropertyMetadataOptions.AffectsRender | FrameworkPropertyMetadataOptions.Inherits);



        [Bindable(true), Category("CornerRadius")]
        [Localizability(LocalizationCategory.None, Readability = Readability.Unreadable)]
        public CornerRadius CornerRadius
        {
            get => (CornerRadius)GetValue(CornerRadiusProperty);
            set => SetValue(CornerRadiusProperty, value);
        }




        #endregion





        #region Icon

        //public static readonly DependencyProperty IconWidthProperty = PropertyAssist.PropertyRegister<HiIcon, double>(p => p.IconWidth, 20d);

        //[Bindable(true), Category("Icon")]
        //[Localizability(LocalizationCategory.None, Readability = Readability.Unreadable)]

        //public double IconWidth
        //{
        //    get => (double)GetValue(IconWidthProperty);
        //    set => SetValue(IconWidthProperty, value);
        //}


        //public static readonly DependencyProperty IconHeightProperty = PropertyAssist.PropertyRegister<HiIcon, double>(p => p.IconHeight, 20d);

        //[Bindable(true), Category("Icon")]
        //[Localizability(LocalizationCategory.None, Readability = Readability.Unreadable)]

        //public double IconHeight
        //{
        //    get => (double)GetValue(IconHeightProperty);
        //    set => SetValue(IconHeightProperty, value);
        //}






        #endregion

    }
}
