using System.ComponentModel;
using System.Windows;
using System.Windows.Controls;

namespace Slight.Core
{

    [DefaultProperty(nameof(ConditionPanel.True))]
    public class ConditionPanel : Canvas
    {
        /// <summary>
        /// 
        /// </summary>
        public static DependencyProperty ConditionProperty = PropertyAssist.PropertyRegister<ConditionPanel, bool?>(i => i.Condition, null, (s, e) =>
        {
            if (e.NewValue == true)
            {
                if (s.True != null)
                {
                    s.True.Visibility = Visibility.Visible;
                }
                if (s.False != null)
                {
                    s.False.Visibility = Visibility.Collapsed;
                }
                return;
            }
            if (e.NewValue == false)
            {
                if (s.True != null)
                {
                    s.True.Visibility = Visibility.Collapsed;
                }
                if (s.False != null)
                {
                    s.False.Visibility = Visibility.Visible;
                }
            }
        });

        [Bindable(true)]
        [Category("Condition")]
        [Localizability(LocalizationCategory.None, Readability = Readability.Unreadable)]
        public bool? Condition
        {
            get => (bool?)base.GetValue(ConditionProperty);
            set => base.SetValue(ConditionProperty, value);
        }

        /// <summary>
        /// 
        /// </summary>
        public static DependencyProperty TrueProperty = PropertyAssist.PropertyRegister<ConditionPanel, UIElement>(i => i.True, null, (s, e) =>
        {
            if (e.OldValue != null && s.Children.Contains(e.OldValue))
            {
                s.Children.Remove(e.OldValue);
            }
            if (s.Condition == true)
            {
                e.NewValue.Visibility = Visibility.Visible;
            }
            else
            {
                e.NewValue.Visibility = Visibility.Collapsed;
            }
            s.Children.Add(e.NewValue);
        });

        [Bindable(true)]
        [Category("True")]
        [Localizability(LocalizationCategory.None, Readability = Readability.Unreadable)]
        public UIElement True
        {
            get => (UIElement)base.GetValue(TrueProperty);
            set => base.SetValue(TrueProperty, value);
        }


        /// <summary>
        /// 
        /// </summary>
        public static DependencyProperty FalseProperty = PropertyAssist.PropertyRegister<ConditionPanel, UIElement>(i => i.False, null, (s, e) =>
        {
            if (e.OldValue != null && s.Children.Contains(e.OldValue))
            {
                s.Children.Remove(e.OldValue);
            }

            if (s.Condition == false)
            {
                e.NewValue.Visibility = Visibility.Visible;
            }
            else
            {
                e.NewValue.Visibility = Visibility.Collapsed;
            }
            s.Children.Add(e.NewValue);
        });

        [Bindable(true)]
        [Category("False")]
        [Localizability(LocalizationCategory.None, Readability = Readability.Unreadable)]
        public UIElement False
        {
            get => (UIElement)base.GetValue(FalseProperty);
            set => base.SetValue(FalseProperty, value);
        }
    }
}
