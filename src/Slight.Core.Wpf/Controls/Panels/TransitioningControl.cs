using System;
using System.ComponentModel;
using System.Diagnostics;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media.Animation;

namespace Slight.Core
{
    public enum TransitionMode : byte
    {
        None = 0,
        RightToLeft = 1,
        LeftToRight = 2,
        BottomToTop = 3,
        TopToBottom = 4,
        RightToLeftWithFade = 5,
        LeftToRightWithFade = 6,
        BottomToTopWithFade = 7,
        TopToBottomWithFade = 8,
        Fade = 9,
        Random = 254,
        Custom = 255
    }


    public class TransitioningControl : ContentControl
    {
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private FrameworkElement contentPresenter;
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private static readonly Random Random = new(Guid.NewGuid().GetHashCode());
        static TransitioningControl()
        {
            PropertyAssist.DefaultStyle<TransitioningControl>(DefaultStyleKeyProperty);
        }
        public TransitioningControl()
        {
            IsVisibleChanged += (s, e) =>
            {
                if (e.NewValue is bool r && r)
                {
                    RunTransition();
                }
            };
            Loaded += (s, e) => RunTransition();
        }


        public static readonly DependencyProperty TransitionModeProperty =
            PropertyAssist.PropertyRegister<TransitioningControl, TransitionMode>(i => i.TransitionMode, TransitionMode.Random,
                FrameworkPropertyMetadataOptions.BindsTwoWayByDefault | FrameworkPropertyMetadataOptions.Inherits,
               (s, e) => s.RunTransition());



        public TransitionMode TransitionMode
        {
            get => (TransitionMode)GetValue(TransitionModeProperty);
            set => SetValue(TransitionModeProperty, value);
        }

        public static readonly DependencyProperty TransitionStoryboardProperty = DependencyProperty.Register(
            "TransitionStoryboard", typeof(Storyboard), typeof(TransitioningControl), new PropertyMetadata(default(Storyboard)));

        public Storyboard TransitionStoryboard
        {
            get => (Storyboard)GetValue(TransitionStoryboardProperty);
            set => SetValue(TransitionStoryboardProperty, value);
        }

        public static DependencyProperty CornerRadiusProperty = PropertyAssist.PropertyRegister<TransitioningControl, CornerRadius>(i => i.CornerRadius, new CornerRadius(0));

        [Bindable(true)]
        [Category("Border")]
        [Localizability(LocalizationCategory.None, Readability = Readability.Unreadable)]
        public CornerRadius CornerRadius
        {
            get => (CornerRadius)base.GetValue(CornerRadiusProperty);
            set => base.SetValue(CornerRadiusProperty, value);
        }

        public void RunTransition()
        {
            if (!IsArrangeValid || contentPresenter == null)
            {
                return;
            }

            TransitionMode mode = TransitionMode;

            if (mode == TransitionMode.None)
            {
                return;
            }
            if (mode == TransitionMode.Custom)
            {
                TransitionStoryboard?.Begin(contentPresenter);
                return;
            }

            if (mode == TransitionMode.Random)
            {
                byte value = (byte)Random.Next(1, 10);
                Invoker.TryCast<TransitionMode>(value, out mode);
            }
            if (mode == TransitionMode.None)
            {
                return;
            }
            Storyboard storyboard = ResourceAssist.GetResource<Storyboard>($"{mode}Transition");
            storyboard?.Begin(contentPresenter);
        }

        public override void OnApplyTemplate()
        {
            base.OnApplyTemplate();

            contentPresenter = GetTemplateChild("PATH_Container") as FrameworkElement;

            //contentPresenter = VisualTreeHelper.GetChild(this, 0) as FrameworkElement;
            //if (contentPresenter is null)
            //{
            //    return;
            //}
            //contentPresenter.RenderTransformOrigin = new Point(0.5, 0.5);
            //contentPresenter.RenderTransform = new TransformGroup
            //{
            //    Children =
            //    {
            //        new ScaleTransform(),
            //        new SkewTransform(),
            //        new RotateTransform(),
            //        new TranslateTransform()
            //    }
            //};
        }
    }
}
