using System;
using System.ComponentModel;
using System.Windows;

namespace Slight.Core
{
    internal class DateTimePicker : System.Windows.Controls.Control
    {
        private const FrameworkPropertyMetadataOptions defaultMetadata = FrameworkPropertyMetadataOptions.AffectsRender | FrameworkPropertyMetadataOptions.Inherits;

        static DateTimePicker()
        {
            PropertyAssist.DefaultStyle<DateTimePicker>(DefaultStyleKeyProperty);
        }

        public static readonly DependencyProperty DateTimeProperty = PropertyAssist.PropertyRegister<DateTimePicker, DateTime>(p => p.DateTime, DateTime.Now, defaultMetadata | FrameworkPropertyMetadataOptions.BindsTwoWayByDefault);



        [Bindable(true), Category("DateTime")]
        [Localizability(LocalizationCategory.None, Readability = Readability.Unreadable)]
        public DateTime DateTime
        {
            get => (DateTime)GetValue(DateTimeProperty);
            set => SetValue(DateTimeProperty, value);
        }

        public static readonly DependencyProperty StringFormatProperty = PropertyAssist.PropertyRegister<DateTimePicker, string>(p => p.StringFormat, "yyyy-MM-dd HH:mm:ss", defaultMetadata);



        [Bindable(true), Category("StringFormat")]
        [Localizability(LocalizationCategory.None, Readability = Readability.Unreadable)]
        public string StringFormat
        {
            get => (string)GetValue(StringFormatProperty);
            set => SetValue(StringFormatProperty, value);
        }



        public static readonly DependencyProperty CornerRadiusProperty = PropertyAssist.PropertyRegister<DateTimePicker, CornerRadius>(p => p.CornerRadius, new CornerRadius(0), defaultMetadata);



        [Bindable(true), Category("CornerRadius")]
        [Localizability(LocalizationCategory.None, Readability = Readability.Unreadable)]
        public CornerRadius CornerRadius
        {
            get => (CornerRadius)GetValue(CornerRadiusProperty);
            set => SetValue(CornerRadiusProperty, value);
        }
    }
}
