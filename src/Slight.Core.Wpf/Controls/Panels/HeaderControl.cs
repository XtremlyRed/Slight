using System.ComponentModel;
using System.Windows;
using System.Windows.Controls;

namespace Slight.Core
{

    /// <summary>
    /// header panel
    /// </summary>
    public class HeaderControl : ContentControl
    {

        static HeaderControl()
        {
            PropertyAssist.DefaultStyle<HeaderControl>(DefaultStyleKeyProperty);
        }

        /// <summary>
        /// create a new instance of  <see cref="HeaderControl"/>
        /// </summary>
        public HeaderControl()
        {

        }

        /// <summary>
        /// 
        /// </summary>
        public static DependencyProperty HeaderProperty = PropertyAssist.PropertyRegister<HeaderControl, object>(i => i.Header, null);

        [Bindable(true)]
        [Category("Header")]
        [Localizability(LocalizationCategory.None, Readability = Readability.Unreadable)]
        public object Header
        {
            get => base.GetValue(HeaderProperty);
            set => base.SetValue(HeaderProperty, value);
        }


        /// <summary>
        /// 
        /// </summary>
        public static DependencyProperty HorizontalHeaderAlignmentProperty = PropertyAssist.PropertyRegister<HeaderControl, HorizontalAlignment>(i => i.HorizontalHeaderAlignment, HorizontalAlignment.Stretch);

        [Bindable(true)]
        [Category("Header")]
        [Localizability(LocalizationCategory.None, Readability = Readability.Unreadable)]
        public HorizontalAlignment HorizontalHeaderAlignment
        {
            get => (HorizontalAlignment)base.GetValue(HorizontalHeaderAlignmentProperty);
            set => base.SetValue(HorizontalHeaderAlignmentProperty, value);
        }


        public static DependencyProperty VerticalHeaderAlignmentProperty = PropertyAssist.PropertyRegister<HeaderControl, VerticalAlignment>(i => i.VerticalHeaderAlignment, VerticalAlignment.Center);

        [Bindable(true)]
        [Category("Header")]
        [Localizability(LocalizationCategory.None, Readability = Readability.Unreadable)]
        public VerticalAlignment VerticalHeaderAlignment
        {
            get => (VerticalAlignment)base.GetValue(VerticalHeaderAlignmentProperty);
            set => base.SetValue(VerticalHeaderAlignmentProperty, value);
        }


        public static DependencyProperty HeaderMarginProperty = PropertyAssist.PropertyRegister<HeaderControl, Thickness>(i => i.HeaderMargin, new Thickness(5, 0, 5, 0));

        [Bindable(true)]
        [Category("Header")]
        [Localizability(LocalizationCategory.None, Readability = Readability.Unreadable)]
        public Thickness HeaderMargin
        {
            get => (Thickness)base.GetValue(HeaderMarginProperty);
            set => base.SetValue(HeaderMarginProperty, value);
        }



        public static DependencyProperty HeaderWidthProperty = PropertyAssist.PropertyRegister<HeaderControl, double>(i => i.HeaderWidth, double.NaN);

        [Bindable(true)]
        [Category("Header")]
        [Localizability(LocalizationCategory.None, Readability = Readability.Unreadable)]
        [TypeConverter(typeof(LengthConverter))]
        public double HeaderWidth
        {
            get => (double)base.GetValue(HeaderWidthProperty);
            set => base.SetValue(HeaderWidthProperty, value);
        }


        public static DependencyProperty HeaderHeightProperty = PropertyAssist.PropertyRegister<HeaderControl, double>(i => i.HeaderHeight, double.NaN);

        [Bindable(true)]
        [Category("Header")]
        [Localizability(LocalizationCategory.None, Readability = Readability.Unreadable)]
        [TypeConverter(typeof(LengthConverter))]
        public double HeaderHeight
        {
            get => (double)base.GetValue(HeaderHeightProperty);
            set => base.SetValue(HeaderHeightProperty, value);
        }




        //public static DependencyProperty HeaderFontWeightProperty = PropertyAssist.PropertyRegister<HeaderPanel, FontWeight>(i => i.HeaderFontWeight, FontWeights.Normal);

        //[Bindable(true)]
        //[Category("Header")]
        //[Localizability(LocalizationCategory.None, Readability = Readability.Unreadable)]
        //public FontWeight HeaderFontWeight
        //{
        //    get => (FontWeight)base.GetValue(HeaderFontWeightProperty);
        //    set => base.SetValue(HeaderFontWeightProperty, value);
        //}




        //public static DependencyProperty HeaderFontSizeProperty = PropertyAssist.PropertyRegister<HeaderPanel, double>(i => i.HeaderFontSize, 16d);

        //[Bindable(true)]
        //[Category("Header")]
        //[Localizability(LocalizationCategory.None, Readability = Readability.Unreadable)]
        //public double HeaderFontSize
        //{
        //    get => (double)base.GetValue(HeaderFontSizeProperty);
        //    set => base.SetValue(HeaderFontSizeProperty, value);
        //}


        //public static DependencyProperty HeaderForegroundProperty = PropertyAssist.PropertyRegister<HeaderPanel, System.Windows.Media.Brush>(i => i.HeaderForeground, System.Windows.Media.Brushes.Black);

        //[Bindable(true)]
        //[Category("Header")]
        //[Localizability(LocalizationCategory.None, Readability = Readability.Unreadable)]
        //public System.Windows.Media.Brush HeaderForeground
        //{
        //    get => (System.Windows.Media.Brush)base.GetValue(HeaderForegroundProperty);
        //    set => base.SetValue(HeaderForegroundProperty, value);
        //}



        //public static DependencyProperty PressedBackgroundProperty = PropertyAssist.PropertyRegister<HeaderControl, Brush>(i => i.PressedBackground, Colors.LightGray.ToBrush());

        //[Bindable(true)]
        //[Category("Cancel")]
        //[Localizability(LocalizationCategory.None, Readability = Readability.Unreadable)]
        //public System.Windows.Media.Brush PressedBackground
        //{
        //    get => (Brush)base.GetValue(PressedBackgroundProperty);
        //    set => base.SetValue(PressedBackgroundProperty, value);
        //}


        //public static DependencyProperty CommandProperty = PropertyAssist.PropertyRegister<HeaderPanel, ICommand>(i => i.Command, null);

        //[Bindable(true)]
        //[Category("Command")]
        //[Localizability(LocalizationCategory.None, Readability = Readability.Unreadable)]
        //public ICommand Command
        //{
        //    get => (ICommand)base.GetValue(CommandProperty);
        //    set => base.SetValue(CommandProperty, value);
        //}

        //public static DependencyProperty CommandParameterProperty = PropertyAssist.PropertyRegister<HeaderPanel, object>(i => i.CommandParameter, null);

        //[Bindable(true)]
        //[Category("Command")]
        //[Localizability(LocalizationCategory.None, Readability = Readability.Unreadable)]
        //public object CommandParameter
        //{
        //    get => base.GetValue(CommandParameterProperty);
        //    set => base.SetValue(CommandParameterProperty, value);
        //}

        //public static readonly RoutedEvent ClickEvent = PropertyAssist.RoutedEvent<HeaderPanel, RoutedEventArgs>(nameof(Click));

        //public event RoutedEventHandler Click
        //{
        //    add { AddHandler(ClickEvent, value); }
        //    remove { RemoveHandler(ClickEvent, value); }
        //}



        ///// <summary>
        ///// Header Background Property
        ///// </summary>
        //public static DependencyProperty HeaderBackgroundProperty = PropertyAssist.PropertyRegister<HeaderPanel, Brush>(i => i.HeaderBackground, Brushes.Transparent);

        ///// <summary>
        ///// Header Background
        ///// </summary>
        //[Bindable(true)]
        //[Category("Background")]
        //[Localizability(LocalizationCategory.None, Readability = Readability.Unreadable)]
        //public Brush HeaderBackground 
        //{
        //    get => (Brush)base.GetValue(HeaderBackgroundProperty);
        //    set => base.SetValue(HeaderBackgroundProperty, value);
        //}







        public static DependencyProperty BackgroundOpacityProperty = PropertyAssist.PropertyRegister<HeaderControl, double>(i => i.BackgroundOpacity, 1d);

        [Bindable(true)]
        [Category("Background")]
        [Localizability(LocalizationCategory.None, Readability = Readability.Unreadable)]
        public double BackgroundOpacity
        {
            get => (double)base.GetValue(BackgroundOpacityProperty);
            set => base.SetValue(BackgroundOpacityProperty, value);
        }



        public static DependencyProperty ContentMarginProperty = PropertyAssist.PropertyRegister<HeaderControl, Thickness>(i => i.ContentMargin, new Thickness(0));

        [Bindable(true)]
        [Category("Content")]
        [Localizability(LocalizationCategory.None, Readability = Readability.Unreadable)]
        public Thickness ContentMargin
        {
            get => (Thickness)base.GetValue(ContentMarginProperty);
            set => base.SetValue(ContentMarginProperty, value);
        }



        public static DependencyProperty CornerRadiusProperty = PropertyAssist.PropertyRegister<HeaderControl, CornerRadius>(i => i.CornerRadius, new CornerRadius(0));

        [Bindable(true)]
        [Category("Border")]
        [Localizability(LocalizationCategory.None, Readability = Readability.Unreadable)]
        public CornerRadius CornerRadius
        {
            get => (CornerRadius)base.GetValue(CornerRadiusProperty);
            set => base.SetValue(CornerRadiusProperty, value);
        }



        public override void OnApplyTemplate()
        {
            base.OnApplyTemplate();

            //if (GetTemplateChild("Path_Click_Container") is Button clickBtn)
            //{
            //    clickBtn.Click += (s, e) =>
            //    {
            //        RoutedEventArgs routedEventArgs = new RoutedEventArgs(ClickEvent, this);
            //        RaiseEvent(routedEventArgs);
            //    };
            //}
        }
    }
}
