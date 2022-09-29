using System.ComponentModel;
using System.Diagnostics;
using System.Windows;
using System.Windows.Data;
using System.Windows.Input;
using System.Windows.Media;

using TextBox1 = Slight.Core.TextBox;
namespace Slight.Core
{

    public delegate void SearchEventHandler(object sender, string searchCondition);

    public class SearchBox : TextBox1, ICommandSource
    {
        static SearchBox()
        {
            PropertyAssist.DefaultStyle<SearchBox>(DefaultStyleKeyProperty);
        }

        public SearchBox()
        {
            Binding binding = new()
            {
                Source = this,
                Path = new PropertyPath(nameof(CommandParameter)),
                Mode = BindingMode.TwoWay,
                UpdateSourceTrigger = UpdateSourceTrigger.PropertyChanged,
            };
            BindingOperations.SetBinding(this, TextProperty, binding);

            AcceptsReturn = false;
        }


        public static DependencyProperty IconMarginProperty = PropertyAssist.PropertyRegister<SearchBox, Thickness>(i => i.IconMargin, new Thickness(5, 0, 5, 0));

        [Bindable(true)]
        [Category("Icon")]
        [Localizability(LocalizationCategory.None, Readability = Readability.Unreadable)]
        public Thickness IconMargin
        {
            get => (Thickness)base.GetValue(IconMarginProperty);
            set => base.SetValue(IconMarginProperty, value);
        }


        public static readonly DependencyProperty IconForegroundProperty = PropertyAssist.PropertyRegister<SearchBox, Brush>(i => i.IconForeground, Brushes.DarkGray);
        [Bindable(true), Category("Icon")]
        [Localizability(LocalizationCategory.None, Readability = Readability.Unreadable)]
        public Brush IconForeground
        {
            get => (Brush)GetValue(IconForegroundProperty);
            set => SetValue(IconForegroundProperty, value);
        }



        public static readonly DependencyProperty IconSizeProperty = PropertyAssist.PropertyRegister<SearchBox, double>(i => i.IconSize, 25);
        [Bindable(true), Category("Icon")]
        [Localizability(LocalizationCategory.None, Readability = Readability.Unreadable)]
        public double IconSize
        {
            get => (double)GetValue(IconSizeProperty);
            set => SetValue(IconSizeProperty, value);
        }

        public static readonly DependencyProperty CommandProperty = PropertyAssist.PropertyRegister<SearchBox, ICommand>(p => p.Command, null);

        [Bindable(true), Category("Command")]
        [Localizability(LocalizationCategory.None, Readability = Readability.Unreadable)]
        public ICommand Command
        {
            get => (ICommand)GetValue(CommandProperty);
            set => SetValue(CommandProperty, value);
        }

        public static readonly DependencyProperty CommandParameterProperty = PropertyAssist.PropertyRegister<SearchBox, object>(p => p.CommandParameter, null);

        [Bindable(true), Category("Command")]
        [Localizability(LocalizationCategory.None, Readability = Readability.Unreadable)]
        public object CommandParameter
        {
            get => GetValue(CommandParameterProperty);
            private set => SetValue(CommandParameterProperty, value);
        }



        public static readonly DependencyProperty CommandTargetProperty = PropertyAssist.PropertyRegister<SearchBox, IInputElement>(p => p.CommandTarget, null);

        [Bindable(true), Category("Command")]
        [Localizability(LocalizationCategory.None, Readability = Readability.Unreadable)]
        public IInputElement CommandTarget
        {
            get => GetValue(CommandTargetProperty) as IInputElement;
            set => SetValue(CommandTargetProperty, value);
        }




        public static readonly DependencyProperty SearchKeyProperty = PropertyAssist.PropertyRegister<SearchBox, Key>(p => p.SearchKey, Key.None);

        [Bindable(true), Category("SearchKey")]
        [Localizability(LocalizationCategory.None, Readability = Readability.Unreadable)]
        public Key SearchKey
        {
            get => (Key)GetValue(SearchKeyProperty);
            set => SetValue(SearchKeyProperty, value);
        }




        public override void OnApplyTemplate()
        {
            base.OnApplyTemplate();

            icon = GetTemplateChild("PART_Click") as Icon;

            if (icon != null)
            {
                icon.Click += (s, e) =>
                {
                    Command.TryExecute(CommandParameter, CommandTarget);
                    SearchClick?.Invoke(this, Text);
                };
            }

            KeyDown += (s, e) =>
            {
                if (SearchKey != Key.None && e.Key == SearchKey)
                {
                    Command.TryExecute(CommandParameter, CommandTarget);
                }
            };
        }


        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private Icon icon;


        /// <summary>
        /// Raised when the popup is opened.
        /// </summary>
        public event SearchEventHandler SearchClick;


        protected override void OnTextChanged(System.Windows.Controls.TextChangedEventArgs e)
        {
            base.OnTextChanged(e);
            AcceptsReturn = false;
        }
    }
}
