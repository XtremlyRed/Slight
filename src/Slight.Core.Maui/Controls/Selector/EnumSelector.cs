

using System.Collections;
using System.ComponentModel;
using System.Diagnostics;
using System.Reflection;

namespace Slight.Core
{
    /// <summary>
    /// An extended combobox that is enumerating Enum values. 
    ///  <para>Use the <see cref="DescriptionAttribute" /> to display entries.</para>
    /// <para>Use the <see cref="BrowsableAttribute" /> to hide specific entries.</para>
    /// </summary>

    public class EnumSelector : Picker
    {
        [DebuggerBrowsable(DebuggerBrowsableState.Never)] private IDictionary<string, object> DisplayNameValueCollention { get; } = new Dictionary<string, object>();
        [DebuggerBrowsable(DebuggerBrowsableState.Never)] private bool isTriggerSelectedChengedEvent = true;


        public EnumSelector()
        {
            SelectedIndexChanged += EnumSelector_SelectedIndexChanged;
            Unloaded += EnumSelector_Unloaded;
        }

        private void EnumSelector_Unloaded(object sender, EventArgs e)
        {
            SelectedIndexChanged -= EnumSelector_SelectedIndexChanged;
            Unloaded -= EnumSelector_Unloaded;
        }




        #region Type property

        public static BindableProperty IgnoreItemsProperty = PropertyAssist.PropertyRegister<EnumSelector, IEnumerable>(i => i.IgnoreItems, (s, e) =>
        {
            s.SetType(s.EnumType, e.NewValue);
        });

        [Bindable(true)]
        [Category("EnumMode")]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]

        public IEnumerable IgnoreItems
        {
            get => (IEnumerable)GetValue(IgnoreItemsProperty);
            set => SetValue(IgnoreItemsProperty, value);
        }

        public static BindableProperty TypeProperty = PropertyAssist.PropertyRegister<EnumSelector, Type>(i => i.EnumType, (s, e) =>
        {
            s.SetType(e.NewValue);
        });

        [Bindable(true)]
        [Category("EnumMode")]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public Type EnumType
        {
            get => (Type)GetValue(TypeProperty);
            set => SetValue(TypeProperty, value);
        }

        public static BindableProperty EnumValueProperty = PropertyAssist.PropertyRegister<EnumSelector, object>(i => i.EnumValue, default, BindingMode.TwoWay, (s, e) =>
        {
            s.SetEnumValue(e.NewValue);
        });

        [Bindable(true)]
        [Category("EnumMode")]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]

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

                ItemsSource = DisplayNameValueCollention.Keys.ToArray();

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


            KeyValuePair<string, object> existResult = DisplayNameValueCollention.FirstOrDefault(i => i.Value.GetHashCode() == enumValue.GetHashCode());

            if (existResult.Key is null || existResult.Value is null)
            {
                existResult = DisplayNameValueCollention.FirstOrDefault();

                Dispatcher.DispatchDelayed(TimeSpan.FromMilliseconds(100), () =>
                {
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

        private void EnumSelector_SelectedIndexChanged(object sender, EventArgs e)
        {
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
