using System.Collections.Concurrent;
using System.ComponentModel;
using System.Diagnostics;

namespace Slight.Core
{
    [ContentProperty(nameof(PopupHost.Content))]
    [DefaultProperty(nameof(PopupHost.Content))]
    public partial class PopupHost : ContentView
    {
        ~PopupHost()
        {

            if (hostName is null)
            {
                return;
            }

            if (popupAwareMapper.TryGetValue(hostName, out PopupHost aware))
            {
                int? code1 = aware?.GetHashCode();
                int code2 = GetHashCode();
                if (code1 == code2)
                {
                    popupAwareMapper.TryRemove(hostName, out PopupHost _);
                }
            }
        }



        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private readonly Thread UiThead = Thread.CurrentThread;

        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private string hostName = string.Empty;

        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private readonly ConcurrentQueue<PopupInfo> PopupInfos = new();

        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        internal static readonly ConcurrentDictionary<string, PopupHost> popupAwareMapper = new();

        public static readonly BindableProperty HostNameProperty =
          PropertyAssist.PropertyRegister<PopupHost, string>(i => i.HostName, null, BindingMode.OneWay, (sender, e) =>
          {
              if (e.OldValue != null)
              {
                  popupAwareMapper.TryRemove(e.OldValue, out PopupHost _);
              }
              if (e.NewValue != null)
              {
                  sender.hostName = e.NewValue;
                  popupAwareMapper[e.NewValue] = sender;
              }
          });

        public string HostName
        {
            get => (string)GetValue(HostNameProperty);
            set => SetValue(HostNameProperty, value);
        }

        public static readonly BindableProperty PopupMessageContainerProperty = PropertyAssist.PropertyRegister<PopupHost, IPopupMessage>(i => i.PopupMessageContainer, null, BindingMode.OneWay);

        public IPopupMessage PopupMessageContainer
        {
            get => (IPopupMessage)GetValue(PopupMessageContainerProperty);
            set => SetValue(PopupMessageContainerProperty, value);
        }

        public static readonly BindableProperty MaskColorProperty =
            PropertyAssist.PropertyRegister<PopupHost, Color>(i => i.MaskColor, Color.FromRgba("#AEAEAE80"), BindingMode.OneWay, (s, e) => { });
        public Color MaskColor
        {
            get => (Color)GetValue(MaskColorProperty);
            set => SetValue(MaskColorProperty, value);
        }


        public static readonly BindableProperty MaskOpacityProperty =
            PropertyAssist.PropertyRegister<PopupHost, double>(i => i.MaskOpacity, 1d, BindingMode.OneWay, (s, e) => { });
        public double MaskOpacity
        {
            get => (double)GetValue(MaskOpacityProperty);
            set => SetValue(MaskOpacityProperty, value);
        }
    }
}
