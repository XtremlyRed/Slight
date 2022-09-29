using System.ComponentModel;
using System.Diagnostics;
using System.Windows;

namespace Slight.Core
{

    public class TextBox : System.Windows.Controls.TextBox
    {
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        internal const string ClickUp = "ClickUp";
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        internal const string ClickDown = "ClickDown";

        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        internal const FrameworkPropertyMetadataOptions render = FrameworkPropertyMetadataOptions.Inherits | FrameworkPropertyMetadataOptions.AffectsRender;

        static TextBox()
        {
            PropertyAssist.DefaultStyle<TextBox>(DefaultStyleKeyProperty);
        }

        public static readonly DependencyProperty CornerRadiusProperty = PropertyAssist.PropertyRegister<TextBox, CornerRadius>(i => i.CornerRadius, new CornerRadius(0));

        [Bindable(true), Category("CornerRadius")]
        [Localizability(LocalizationCategory.None, Readability = Readability.Unreadable)]
        public CornerRadius CornerRadius
        {
            get => (CornerRadius)GetValue(CornerRadiusProperty);
            set => SetValue(CornerRadiusProperty, value);
        }


        #region Placeholder

        public static readonly DependencyProperty PlaceholderOpacityProperty = PropertyAssist.PropertyRegister<TextBox, double>(i => i.PlaceholderOpacity, 0.6d);

        [Bindable(true), Category("Placeholder")]
        [Localizability(LocalizationCategory.None, Readability = Readability.Unreadable)]
        public double PlaceholderOpacity
        {
            get => ((double)GetValue(PlaceholderOpacityProperty)).FromRange(0, 1);
            set => SetValue(PlaceholderOpacityProperty, value.FromRange(0, 1));
        }



        public static readonly DependencyProperty PlaceholderProperty = PropertyAssist.PropertyRegister<TextBox, object>(i => i.Placeholder);

        [Bindable(true), Category("Placeholder")]
        [Localizability(LocalizationCategory.None, Readability = Readability.Unreadable)]
        public object Placeholder
        {
            get => GetValue(PlaceholderProperty);
            set => SetValue(PlaceholderProperty, value);
        }

        public static readonly DependencyProperty PlaceholderPaddingProperty = PropertyAssist.PropertyRegister<TextBox, Thickness>(i => i.PlaceholderPadding, new Thickness(7, 0, 7, 0));

        [Bindable(true), Category("Placeholder")]
        [Localizability(LocalizationCategory.None, Readability = Readability.Unreadable)]
        public Thickness PlaceholderPadding
        {
            get => (Thickness)GetValue(PlaceholderPaddingProperty);
            set => SetValue(PlaceholderPaddingProperty, value);
        }


        public static readonly DependencyProperty PlaceholderVerticalAlignmentProperty =
            PropertyAssist.PropertyRegister<TextBox, VerticalAlignment>(i => i.PlaceholderVerticalAlignment, VerticalAlignment.Center);

        [Bindable(true), Category("Placeholder")]
        [Localizability(LocalizationCategory.None, Readability = Readability.Unreadable)]

        public VerticalAlignment PlaceholderVerticalAlignment
        {
            get => (VerticalAlignment)GetValue(PlaceholderVerticalAlignmentProperty);
            set => SetValue(PlaceholderVerticalAlignmentProperty, value);
        }


        public static readonly DependencyProperty PlaceholderHorizontalAlignmentProperty =
            PropertyAssist.PropertyRegister<TextBox, HorizontalAlignment>(i => i.PlaceholderHorizontalAlignment, HorizontalAlignment.Left);

        [Bindable(true), Category("Placeholder")]
        [Localizability(LocalizationCategory.None, Readability = Readability.Unreadable)]

        public HorizontalAlignment PlaceholderHorizontalAlignment
        {
            get => (HorizontalAlignment)GetValue(PlaceholderHorizontalAlignmentProperty);
            set => SetValue(PlaceholderHorizontalAlignmentProperty, value);
        }


        public static readonly DependencyProperty SelectedAllOnFocusProperty =
        PropertyAssist.PropertyRegister<TextBox, bool>(i => i.SelectedAllOnFocus, false);

        [Bindable(true), Category("OnFocus")]
        [Localizability(LocalizationCategory.None, Readability = Readability.Unreadable)]
        public bool SelectedAllOnFocus
        {
            get => (bool)GetValue(SelectedAllOnFocusProperty);
            set => SetValue(SelectedAllOnFocusProperty, value);
        }


        #endregion Placeholder




        protected override void OnGotFocus(RoutedEventArgs e)
        {
            base.OnGotFocus(e);
            if (IsEnabled && SelectedAllOnFocus)
            {
                if (Text != null && Text.Length > 0)
                {
                    Select(0, Text.Length);
                }
            }
        }
    }
}
