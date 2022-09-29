using System;
using System.ComponentModel;
using System.Diagnostics;
using System.Windows;
using System.Windows.Input;

using Slight.Core.Interactivity;

namespace Slight.Core
{

    public class EventRaiser : FrameworkElement, IDisposable
    {
        [DebuggerBrowsable(DebuggerBrowsableState.Never)] private DependencyObject attachObject;
        [DebuggerBrowsable(DebuggerBrowsableState.Never)] private DelegateProxy delegateproxy;

        public EventRaiser()
        {
            attachObject = this;
        }

        private static readonly DependencyProperty EventMameProperty = PropertyAssist.PropertyRegister<EventRaiser, string>(i => i.EventMame, null, (s, e) =>
        {
            s.EventToCommand(e.NewValue, s.Command);
        });

        [Bindable(true), Category("EventMame")]
        [Localizability(LocalizationCategory.None, Readability = Readability.Unreadable)]

        public string EventMame
        {
            get => GetValue(EventMameProperty) as string;
            set => SetValue(EventMameProperty, value);
        }

        private static readonly DependencyProperty CommandProperty = PropertyAssist.PropertyRegister<EventRaiser, ICommand>(i => i.Command, null, (s, e) =>
        {
            s.EventToCommand(s.EventMame, e.NewValue);
        });

        [Bindable(true), Category("Command")]
        [Localizability(LocalizationCategory.None, Readability = Readability.Unreadable)]

        public ICommand Command
        {
            get => GetValue(CommandProperty) as ICommand;
            set => SetValue(CommandProperty, value);
        }

        private static readonly DependencyProperty CommandParameterProperty = PropertyAssist.PropertyRegister<EventRaiser, object>(i => i.CommandParameter, null);

        [Bindable(true), Category("CommandParameter")]
        [Localizability(LocalizationCategory.None, Readability = Readability.Unreadable)]

        public object CommandParameter
        {
            get => GetValue(CommandParameterProperty);
            set => SetValue(CommandParameterProperty, value);
        }

        private static readonly DependencyProperty PushEventArgsToCommandParameterProperty = PropertyAssist.PropertyRegister<EventRaiser, bool>(i => i.PushEventArgsToCommandParameter, false);

        [Bindable(true), Category("PushEventArgsToCommandParameter")]
        [Localizability(LocalizationCategory.None, Readability = Readability.Unreadable)]

        public bool PushEventArgsToCommandParameter
        {
            get => (bool)GetValue(PushEventArgsToCommandParameterProperty);
            set => SetValue(PushEventArgsToCommandParameterProperty, value);
        }

        private void EventToCommand(string eventName, ICommand command)
        {
            if (attachObject is null || eventName is null || command is null)
            {
                return;
            }

            delegateproxy = new DelegateProxy(attachObject, this);
            delegateproxy.AttacheHandler();

        }

        public void Attach(DependencyObject dependencyObject)
        {
            if (Equals(dependencyObject, attachObject))
            {
                return;
            }

            attachObject = dependencyObject;

            if (dependencyObject is FrameworkElement framework)
            {
                DataContext = framework.DataContext;
                framework.DataContextChanged += EventRaiser_DataContextChanged;
                return;
            }
        }

        public void Dispose()
        {
            if (attachObject is not FrameworkElement framework)
            {
                return;
            }

            delegateproxy?.Dispose();
            framework.DataContextChanged -= EventRaiser_DataContextChanged;
            attachObject = null;
            delegateproxy = null;
        }
        private void EventRaiser_DataContextChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            DataContext = e.NewValue;
        }
    }
}
