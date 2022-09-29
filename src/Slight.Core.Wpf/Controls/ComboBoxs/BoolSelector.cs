using System.Collections;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Windows;
using System.Windows.Controls;

namespace Slight.Core
{
    public class BoolSelector : ComboBox
    {

        public const string YesDisplayValue = "Yes";
        public const string NoDisplayValue = "No";


        [DebuggerDisplay("{Value}")]
        private class KeyValue : INotifyPropertyChanged
        {
            public int Key { get; set; }
            [DebuggerBrowsable(DebuggerBrowsableState.Never)] private string value;
            public string Value
            {
                get => value;
                set
                {
                    this.value = value;
                    PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Value)));
                }
            }
            public event PropertyChangedEventHandler PropertyChanged;


        }


        static BoolSelector()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(BoolSelector),
                new FrameworkPropertyMetadata(typeof(ComboBox)));
        }
        [DebuggerBrowsable(DebuggerBrowsableState.Never)] private readonly ObservableCollection<KeyValue> source;

        public BoolSelector()
        {
            DisplayMemberPath = nameof(KeyValue.Value);
            source = new ObservableCollection<KeyValue>
            {
                new KeyValue()
                {
                    Key = 0,
                    Value = YesDisplay
                },
                new KeyValue()
                {
                    Key = 1,
                    Value = NoDisplay
                }
            };
            ItemsSource = source;
            SelectedItem = source.LastOrDefault();
        }

        #region display property


        public static readonly DependencyProperty CornerRadiusProperty = PropertyAssist.PropertyRegister<BoolSelector, CornerRadius>(i => i.CornerRadius, new CornerRadius(0));

        [Bindable(true), Category("CornerRadius")]
        [Localizability(LocalizationCategory.None, Readability = Readability.Unreadable)]
        public CornerRadius CornerRadius
        {
            get => (CornerRadius)GetValue(CornerRadiusProperty);
            set => SetValue(CornerRadiusProperty, value);
        }



        public static DependencyProperty YesDisplayProperty = PropertyAssist.PropertyRegister<BoolSelector, string>(i => i.YesDisplay, YesDisplayValue, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault | FrameworkPropertyMetadataOptions.Inherits, (s, e) =>
        {
            KeyValue f = s.source.FirstOrDefault(i => i.Key == 0);
            f.Value = string.IsNullOrWhiteSpace(e.NewValue) ? YesDisplayValue : e.NewValue;
            if (s.Value == true)
            {
                s.Text = f.Value;
            }
        });

        [Bindable(true)]
        [Category("DisplayValue")]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        [Localizability(LocalizationCategory.NeverLocalize)]
        public string YesDisplay
        {
            get => (string)GetValue(YesDisplayProperty);
            set => SetValue(YesDisplayProperty, value);
        }

        public static DependencyProperty NoDisplayProperty = PropertyAssist.PropertyRegister<BoolSelector, string>(i => i.NoDisplay, NoDisplayValue, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault | FrameworkPropertyMetadataOptions.Inherits, (s, e) =>
         {
             KeyValue f = s.source.FirstOrDefault(i => i.Key == 1);
             f.Value = string.IsNullOrWhiteSpace(e.NewValue) ? NoDisplayValue : e.NewValue;
             if (s.Value == false)
             {
                 s.Text = f.Value;
             }
         });


        [Bindable(true)]
        [Category("DisplayValue")]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        [Localizability(LocalizationCategory.NeverLocalize)]
        public string NoDisplay
        {
            get => (string)GetValue(NoDisplayProperty);
            set => SetValue(NoDisplayProperty, value);
        }




        public static DependencyProperty ValueProperty = PropertyAssist.PropertyRegister<BoolSelector, bool>(i => i.Value, false, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault | FrameworkPropertyMetadataOptions.Inherits, System.Windows.Data.UpdateSourceTrigger.PropertyChanged, (s, e) =>
        {
            int a = e.NewValue ? 0 : 1;
            KeyValue keyValue = s.source.FirstOrDefault(i => i.Key == a);

            if (s.SelectedItem is KeyValue keyValue1)
            {
                if (keyValue1.Key != keyValue.Key)
                {
                    s.SelectedItem = keyValue;
                }
                return;
            }
            s.SelectedItem = keyValue;
        });


        [Bindable(true)]
        [Category("ValueMode")]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        [Localizability(LocalizationCategory.NeverLocalize)]
        public bool Value
        {
            get => (bool)GetValue(ValueProperty);
            set => SetValue(ValueProperty, value);
        }

        #endregion

        private new IEnumerable ItemsSource
        {
            get => base.ItemsSource;
            set => base.ItemsSource = value;
        }

        private new object SelectedItem
        {
            get => base.SelectedItem;
            set => base.SelectedItem = value;
        }

        private new readonly DependencyProperty ItemsSourceProperty = ComboBox.ItemsSourceProperty;

        private new readonly DependencyProperty SelectedItemProperty = ComboBox.SelectedItemProperty;


        protected override void OnSelectionChanged(SelectionChangedEventArgs e)
        {
            base.OnSelectionChanged(e);
            KeyValue item = source.FirstOrDefault(i => i.Key == SelectedIndex);
            if (item is null)
            {
                return;
            }
            bool v = item.Key == 0;
            if (Value == v)
            {
                return;
            }
            Value = v;
        }
    }
}
