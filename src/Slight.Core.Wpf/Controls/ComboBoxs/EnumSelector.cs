using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace Slight.Core
{
    /// <summary>
    /// An extended combobox that is enumerating Enum values. 
    ///  <para>Use the <see cref="DescriptionAttribute" /> to display entries.</para>
    /// <para>Use the <see cref="BrowsableAttribute" /> to hide specific entries.</para>
    /// </summary>

    public class EnumSelector : ComboBox
    {
        [DebuggerBrowsable(DebuggerBrowsableState.Never)] private IDictionary<string, object> DisplayNameValueCollention { get; } = new Dictionary<string, object>();
        [DebuggerBrowsable(DebuggerBrowsableState.Never)] private bool isTriggerSelectedChengedEvent = true;
        static EnumSelector()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(EnumSelector),
                new FrameworkPropertyMetadata(typeof(ComboBox)));
        }
        public new bool IsEditable { get => false; set => base.IsEditable = false; }

        #region Type property

        public static readonly DependencyProperty CornerRadiusProperty = PropertyAssist.PropertyRegister<EnumSelector, CornerRadius>(i => i.CornerRadius, new CornerRadius(0));

        [Bindable(true), Category("CornerRadius")]
        [Localizability(LocalizationCategory.None, Readability = Readability.Unreadable)]
        public CornerRadius CornerRadius
        {
            get => (CornerRadius)GetValue(CornerRadiusProperty);
            set => SetValue(CornerRadiusProperty, value);
        }

        public static DependencyProperty IgnoreItemsProperty = PropertyAssist.PropertyRegister<EnumSelector, IEnumerable>(i => i.IgnoreItems, (s, e) =>
        {
            s.SetType(s.EnumType, e.NewValue);
        });

        [Bindable(true)]
        [Category("EnumMode")]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        [Localizability(LocalizationCategory.NeverLocalize)]
        public IEnumerable IgnoreItems
        {
            get => (IEnumerable)GetValue(IgnoreItemsProperty);
            set => SetValue(IgnoreItemsProperty, value);
        }

        public static DependencyProperty TypeProperty = PropertyAssist.PropertyRegister<EnumSelector, Type>(i => i.EnumType, (s, e) =>
        {
            s.SetType(e.NewValue);
        });

        [Bindable(true)]
        [Category("EnumMode")]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        [Localizability(LocalizationCategory.NeverLocalize)]
        public Type EnumType
        {
            get => (Type)GetValue(TypeProperty);
            set => SetValue(TypeProperty, value);
        }

        public static DependencyProperty EnumValueProperty = PropertyAssist.PropertyRegister<EnumSelector, object>(i => i.EnumValue, default, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault | FrameworkPropertyMetadataOptions.Inherits, System.Windows.Data.UpdateSourceTrigger.PropertyChanged, (s, e) =>
        {
            s.SetEnumValue(e.NewValue);
        });

        [Bindable(true)]
        [Category("EnumMode")]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        [Localizability(LocalizationCategory.NeverLocalize)]
        public object EnumValue
        {
            get => GetValue(EnumValueProperty);
            set => SetValue(EnumValueProperty, value);
        }

        #endregion

        private void SetType(Type enumType, IEnumerable removeArray = null)
        {
            try
            {
                DisplayNameValueCollention.Clear();
                List<FieldInfo> list = enumType.GetFields().Where(i => i.IsStatic && !i.IsSpecialName).ToList();

                List<object> removeArray2 = removeArray?.Cast<object>().Where(i => i != null).ToList();

                foreach (FieldInfo fieldInfo in list)
                {
                    object value = fieldInfo.GetValue(null);

                    if (removeArray2?.Any(i => i.GetHashCode() == value.GetHashCode()) ?? false)
                    {
                        continue;
                    }

                    object[] attributes = fieldInfo.GetCustomAttributes(false);

                    BrowsableAttribute browsable = attributes.OfType<BrowsableAttribute>().FirstOrDefault();

                    if (browsable != null && browsable.Browsable == false)
                    {
                        continue;
                    }

                    string displayName = attributes.OfType<DisplayNameAttribute>().FirstOrDefault()?.DisplayName;

                    if (string.IsNullOrWhiteSpace(displayName))
                    {
                        displayName = attributes.OfType<DescriptionAttribute>().FirstOrDefault()?.Description;

                        if (string.IsNullOrWhiteSpace(displayName))
                        {
                            displayName = fieldInfo.Name;
                        }
                    }

                    if (DisplayNameValueCollention.ContainsKey(displayName))
                    {
                        continue;
                    }

                    DisplayNameValueCollention[displayName] = value;
                }

                ItemsSource = DisplayNameValueCollention.Keys.ToObservableCollection();

                if (EnumValue != null && SelectedItem is null)
                {
                    SetEnumValue(EnumValue);
                }
            }
            catch
            {
                // ignore
            }
        }

        private void SetEnumValue(object enumValue)
        {

            if (enumValue is null || DisplayNameValueCollention.Count == 0)
            {
                return;
            }

            base.IsEditable = false;

            KeyValuePair<string, object> existResult = DisplayNameValueCollention.FirstOrDefault(i => i.Value.GetHashCode() == enumValue.GetHashCode());

            if (existResult.Key is null || existResult.Value is null)
            {
                existResult = DisplayNameValueCollention.FirstOrDefault();

                Dispatcher.InvokeAsync(async () =>
                {
                    await Task.Delay(0100);
                    SelectedItem = existResult.Key;
                });
                return;
            }

            if ((SelectedItem as string) == existResult.Key)
            {
                return;
            }

            try
            {
                isTriggerSelectedChengedEvent = false;

                SelectedItem = existResult.Key;
            }
            finally
            {
                isTriggerSelectedChengedEvent = true;
            }
        }

        protected override void OnSelectionChanged(SelectionChangedEventArgs e)
        {
            base.OnSelectionChanged(e);

            if (isTriggerSelectedChengedEvent == true)
            {
                string key = SelectedItem as string;

                if (DisplayNameValueCollention.TryGetValue(key, out object value))
                {
                    EnumValue = value;
                }
            }
        }
    }
}
