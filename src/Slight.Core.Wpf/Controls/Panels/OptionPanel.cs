using System.ComponentModel;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;

namespace Slight.Core
{
    public class OptionPanel : Control
    {

        public const string CancelName = "Cancel";
        public const string ConfirmName = "Confirm";
        private static readonly object cancelContent = "Cancel";
        private static readonly object confirmContent = "Confirm";
        static OptionPanel()
        {
            PropertyAssist.DefaultStyle<OptionPanel>(DefaultStyleKeyProperty);
        }
        public OptionPanel()
        {

        }

        public static DependencyProperty CornerRadiusProperty = PropertyAssist.PropertyRegister<OptionPanel, CornerRadius>(i => i.CornerRadius, new CornerRadius(0));

        [Bindable(true)]
        [Category("Border")]
        [Localizability(LocalizationCategory.None, Readability = Readability.Unreadable)]
        public CornerRadius CornerRadius
        {
            get => (CornerRadius)base.GetValue(CornerRadiusProperty);
            set => base.SetValue(CornerRadiusProperty, value);
        }



        public static DependencyProperty CancelMarginProperty = PropertyAssist.PropertyRegister<OptionPanel, Thickness>(i => i.CancelMargin, new Thickness(0));

        [Bindable(true)]
        [Category("Cancel")]
        [Localizability(LocalizationCategory.None, Readability = Readability.Unreadable)]
        public Thickness CancelMargin
        {
            get => (Thickness)base.GetValue(CancelMarginProperty);
            set => base.SetValue(CancelMarginProperty, value);
        }



        public static DependencyProperty CancelWidthProperty = PropertyAssist.PropertyRegister<OptionPanel, double>(i => i.CancelWidth, 100d);

        [Bindable(true)]
        [Category("Cancel")]
        [Localizability(LocalizationCategory.None, Readability = Readability.Unreadable)]
        [TypeConverter(typeof(LengthConverter))]
        public double CancelWidth
        {
            get => (double)base.GetValue(CancelWidthProperty);
            set => base.SetValue(CancelWidthProperty, value);
        }


        public static DependencyProperty CancelForegroundProperty = PropertyAssist.PropertyRegister<OptionPanel, Brush>(i => i.CancelForeground, Colors.OrangeRed.ToBrush());

        [Bindable(true)]
        [Category("Cancel")]
        [Localizability(LocalizationCategory.None, Readability = Readability.Unreadable)]
        public System.Windows.Media.Brush CancelForeground
        {
            get => (Brush)base.GetValue(CancelForegroundProperty);
            set => base.SetValue(CancelForegroundProperty, value);
        }
        public static DependencyProperty CancelBackgroundProperty = PropertyAssist.PropertyRegister<OptionPanel, Brush>(i => i.CancelBackground, Colors.LightGray.ToBrush());

        [Bindable(true)]
        [Category("Cancel")]
        [Localizability(LocalizationCategory.None, Readability = Readability.Unreadable)]
        public System.Windows.Media.Brush CancelBackground
        {
            get => (Brush)base.GetValue(CancelBackgroundProperty);
            set => base.SetValue(CancelBackgroundProperty, value);
        }

        public static DependencyProperty CancelPressedBackgroundProperty = PropertyAssist.PropertyRegister<OptionPanel, Brush>(i => i.CancelPressedBackground, Colors.LightGray.ToBrush());

        [Bindable(true)]
        [Category("Cancel")]
        [Localizability(LocalizationCategory.None, Readability = Readability.Unreadable)]
        public System.Windows.Media.Brush CancelPressedBackground
        {
            get => (Brush)base.GetValue(CancelPressedBackgroundProperty);
            set => base.SetValue(CancelPressedBackgroundProperty, value);
        }

        public static DependencyProperty CancelContentProperty = PropertyAssist.PropertyRegister<OptionPanel, object>(i => i.CancelContent, cancelContent);

        [Bindable(true)]
        [Category("Cancel")]
        [Localizability(LocalizationCategory.None, Readability = Readability.Unreadable)]
        public object CancelContent
        {
            get => base.GetValue(CancelContentProperty);
            set => base.SetValue(CancelContentProperty, value);
        }



        public static DependencyProperty CancelCommandProperty = PropertyAssist.PropertyRegister<OptionPanel, ICommand>(i => i.CancelCommand, null);

        [Bindable(true)]
        [Category("Cancel")]
        [Localizability(LocalizationCategory.None, Readability = Readability.Unreadable)]
        public ICommand CancelCommand
        {
            get => (ICommand)base.GetValue(CancelCommandProperty);
            set => base.SetValue(CancelCommandProperty, value);
        }


        public static DependencyProperty CancelCommandParameterProperty = PropertyAssist.PropertyRegister<OptionPanel, object>(i => i.CancelCommandParameter, null);

        [Bindable(true)]
        [Category("Cancel")]
        [Localizability(LocalizationCategory.None, Readability = Readability.Unreadable)]
        public object CancelCommandParameter
        {
            get => base.GetValue(CancelCommandParameterProperty);
            set => base.SetValue(CancelCommandParameterProperty, value);
        }


        public static readonly DependencyProperty CancelCommandTargetProperty = PropertyAssist.PropertyRegister<OptionPanel, IInputElement>(p => p.CancelCommandTarget, null);

        [Bindable(true), Category("Cancel")]
        [Localizability(LocalizationCategory.None, Readability = Readability.Unreadable)]
        public IInputElement CancelCommandTarget
        {
            get => GetValue(CancelCommandTargetProperty) as IInputElement;
            set => SetValue(CancelCommandTargetProperty, value);
        }


        public static DependencyProperty ConfirmMarginProperty = PropertyAssist.PropertyRegister<OptionPanel, Thickness>(i => i.ConfirmMargin, new Thickness(0));

        [Bindable(true)]
        [Category("Confirm")]
        [Localizability(LocalizationCategory.None, Readability = Readability.Unreadable)]
        public Thickness ConfirmMargin
        {
            get => (Thickness)base.GetValue(ConfirmMarginProperty);
            set => base.SetValue(ConfirmMarginProperty, value);
        }



        public static DependencyProperty ConfirmWidthProperty = PropertyAssist.PropertyRegister<OptionPanel, double>(i => i.ConfirmWidth, 100d);

        [Bindable(true)]
        [Category("Confirm")]
        [Localizability(LocalizationCategory.None, Readability = Readability.Unreadable)]
        [TypeConverter(typeof(LengthConverter))]
        public double ConfirmWidth
        {
            get => (double)base.GetValue(ConfirmWidthProperty);
            set => base.SetValue(ConfirmWidthProperty, value);
        }
        public static DependencyProperty ConfirmForegroundProperty = PropertyAssist.PropertyRegister<OptionPanel, Brush>(i => i.ConfirmForeground, Colors.Green.ToBrush());
        [Bindable(true)]
        [Category("Confirm")]
        [Localizability(LocalizationCategory.None, Readability = Readability.Unreadable)]
        public System.Windows.Media.Brush ConfirmForeground
        {
            get => (Brush)base.GetValue(ConfirmForegroundProperty);
            set => base.SetValue(ConfirmForegroundProperty, value);
        }



        public static DependencyProperty ConfirmBackgroundProperty = PropertyAssist.PropertyRegister<OptionPanel, Brush>(i => i.ConfirmBackground, Colors.LightGray.ToBrush());
        [Bindable(true)]
        [Category("Confirm")]
        [Localizability(LocalizationCategory.None, Readability = Readability.Unreadable)]
        public System.Windows.Media.Brush ConfirmBackground
        {
            get => (Brush)base.GetValue(ConfirmBackgroundProperty);
            set => base.SetValue(ConfirmBackgroundProperty, value);
        }

        public static DependencyProperty ConfirmPressedBackgroundProperty = PropertyAssist.PropertyRegister<OptionPanel, Brush>(i => i.ConfirmPressedBackground, Colors.LightGray.ToBrush());
        [Bindable(true)]
        [Category("Confirm")]
        [Localizability(LocalizationCategory.None, Readability = Readability.Unreadable)]
        public System.Windows.Media.Brush ConfirmPressedBackground
        {
            get => (Brush)base.GetValue(ConfirmPressedBackgroundProperty);
            set => base.SetValue(ConfirmPressedBackgroundProperty, value);
        }

        public static DependencyProperty ConfirmContentProperty = PropertyAssist.PropertyRegister<OptionPanel, object>(i => i.ConfirmContent, confirmContent);

        [Bindable(true)]
        [Category("Confirm")]
        [Localizability(LocalizationCategory.None, Readability = Readability.Unreadable)]
        public object ConfirmContent
        {
            get => base.GetValue(ConfirmContentProperty);
            set => base.SetValue(ConfirmContentProperty, value);
        }



        public static DependencyProperty ConfirmCommandProperty = PropertyAssist.PropertyRegister<OptionPanel, ICommand>(i => i.ConfirmCommand, null);

        [Bindable(true)]
        [Category("Confirm")]
        [Localizability(LocalizationCategory.None, Readability = Readability.Unreadable)]
        public ICommand ConfirmCommand
        {
            get => (ICommand)base.GetValue(ConfirmCommandProperty);
            set => base.SetValue(ConfirmCommandProperty, value);
        }


        public static DependencyProperty ConfirmCommandParameterProperty = PropertyAssist.PropertyRegister<OptionPanel, object>(i => i.ConfirmCommandParameter, null);

        [Bindable(true)]
        [Category("Confirm")]
        [Localizability(LocalizationCategory.None, Readability = Readability.Unreadable)]
        public object ConfirmCommandParameter
        {
            get => base.GetValue(ConfirmCommandParameterProperty);
            set => base.SetValue(ConfirmCommandParameterProperty, value);
        }

        public static readonly DependencyProperty ConfirmCommandTargetProperty = PropertyAssist.PropertyRegister<OptionPanel, IInputElement>(p => p.ConfirmCommandTarget, null);

        [Bindable(true), Category("Cancel")]
        [Localizability(LocalizationCategory.None, Readability = Readability.Unreadable)]
        public IInputElement ConfirmCommandTarget
        {
            get => GetValue(ConfirmCommandTargetProperty) as IInputElement;
            set => SetValue(ConfirmCommandTargetProperty, value);
        }


        private bool isApplyed;

        // private readonly IDictionary<object, DateTime> clickMapper = new Dictionary<object, DateTime>();
        public override void OnApplyTemplate()
        {
            base.OnApplyTemplate();

            if (isApplyed)
            {
                return;
            }
            isApplyed = true;

            if (GetTemplateChild(CancelName) is Button cancel)
            {
                cancel.Click += (s, e) =>
                {

                    CancelCommand.TryExecute(CancelCommandParameter, CancelCommandTarget);


                    RoutedEventArgs routedEventArgs = new(CancelClickEvent, this);
                    RaiseEvent(routedEventArgs);
                };
            }

            if (GetTemplateChild(ConfirmName) is Button confirm)
            {
                confirm.Click += (s, e) =>
                {
                    ConfirmCommand.TryExecute(ConfirmCommandParameter, ConfirmCommandTarget);

                    RoutedEventArgs routedEventArgs = new(ConfirmClickEvent, this);
                    RaiseEvent(routedEventArgs);
                };
            }
        }


        public static readonly RoutedEvent CancelClickEvent = PropertyAssist.RoutedEvent<OptionPanel, RoutedEventArgs>(nameof(CancelClick));

        public event RoutedEventHandler CancelClick
        {
            add => AddHandler(CancelClickEvent, value);
            remove => RemoveHandler(CancelClickEvent, value);
        }


        public static readonly RoutedEvent ConfirmClickEvent = PropertyAssist.RoutedEvent<OptionPanel, RoutedEventArgs>(nameof(ConfirmClick));

        public event RoutedEventHandler ConfirmClick
        {
            add => AddHandler(ConfirmClickEvent, value);
            remove => RemoveHandler(ConfirmClickEvent, value);
        }
    }
}
