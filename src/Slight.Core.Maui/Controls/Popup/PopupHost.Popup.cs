

using System.Diagnostics;

namespace Slight.Core
{
    public partial class PopupHost
    {
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private bool isPopuped;


        public bool CheckAccess()
        {
            return UiThead.ManagedThreadId == Thread.CurrentThread.ManagedThreadId;
        }


        public Task ShowAsync(string message, PopupConfig config = null)
        {
            string hostName = CheckAccess() ? HostName : Dispatcher.Invoke(() => HostName);

            return ShowAsync(hostName, message, config);
        }

        public Task<bool> ConfirmAsync(string message, PopupConfig config = null)
        {
            string hostName = CheckAccess() ? HostName : Dispatcher.Invoke(() => HostName);
            return ConfirmAsync(hostName, message, config);
        }

        public Task<bool> PopupAsync<Target>(Func<Target> uIElementFunc, PopupConfig config = null)
            where Target : View, IPopupContent
        {
            string hostName = CheckAccess() ? HostName : Dispatcher.Invoke(() => HostName);
            return PopupAsync<Target>(hostName, uIElementFunc, config);
        }

        public static Task ShowAsync(string popupHostName, string message, PopupConfig config = null)
        {
            if (string.IsNullOrWhiteSpace(popupHostName))
            {
                throw new ArgumentException(nameof(popupHostName));
            }

            popupAwareMapper.TryGetValue(popupHostName, out PopupHost popup);

            if (popup != null)
            {
                PopupInfo PopupInfo = new(popupHostName, PopupMode.Show, message, null, config);

                popup.PopupWindow(PopupInfo);

                return PopupInfo.DisplayAsync();
            }
            throw new ArgumentException($"target PopupHost with the name :{popupHostName} not exists");

        }

        public static Task<bool> ConfirmAsync(string popupHostName, string message, PopupConfig config = null)
        {
            if (string.IsNullOrWhiteSpace(popupHostName))
            {
                throw new ArgumentException(nameof(popupHostName));
            }

            popupAwareMapper.TryGetValue(popupHostName, out PopupHost popup);

            if (popup != null)
            {
                PopupInfo PopupInfo = new(popupHostName, PopupMode.Confirm, message, null, config);

                popup.PopupWindow(PopupInfo);

                return PopupInfo.DisplayAsync();
            }

            throw new ArgumentException($"target PopupHost with the name :{popupHostName} not exists");

        }

        public static Task<bool> PopupAsync<Target>(string popupHostName, Func<Target> uIElementFunc, PopupConfig config = null)
            where Target : View, IPopupContent

        {
            if (string.IsNullOrWhiteSpace(popupHostName))
            {
                throw new ArgumentException(nameof(popupHostName));
            }

            if (uIElementFunc is null)
            {
                throw new ArgumentNullException(nameof(uIElementFunc));
            }

            popupAwareMapper.TryGetValue(popupHostName, out PopupHost popup);

            if (popup != null)
            {
                PopupInfo PopupInfo = new(popupHostName, PopupMode.Popup, null, uIElementFunc, config);

                popup.PopupWindow(PopupInfo);

                return PopupInfo.DisplayAsync();
            }

            throw new ArgumentException($"target PopupHost with the name :{popupHostName} not exists");

        }



        private void PopupWindow(PopupInfo popupInfo)
        {
            lock (PopupInfos)
            {
                PopupInfos.Enqueue(popupInfo);

                if (isPopuped)
                {
                    return;
                }

                isPopuped = true;
            }

            if (CheckAccess())
            {
                PopupLogic().NoAwaiter();
                return;
            }
            Dispatcher.Dispatch(async () => await PopupLogic());
        }
    }
}
