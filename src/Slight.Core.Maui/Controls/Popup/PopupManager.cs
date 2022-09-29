namespace Slight.Core
{
    public class PopupManager : IPopupManager
    {
        public Task ShowAsync(string popupHostName, string message, PopupConfig config = null)
        {
            return PopupHost.ShowAsync(popupHostName, message, config);
        }
        public Task<bool> ConfirmAsync(string popupHostName, string message, PopupConfig config = null)
        {
            return PopupHost.ConfirmAsync(popupHostName, message, config);
        }
        public Task<bool> PopupAsync<Target>(string popupHostName, Func<Target> uIElementFunc, PopupConfig config = null)
             where Target : View, IPopupContent
        {
            return PopupHost.PopupAsync(popupHostName, uIElementFunc, config);
        }


        public IPopupAware Aware(string popupHostName)
        {
            if (string.IsNullOrWhiteSpace(popupHostName))
            {
                throw new ArgumentNullException(nameof(popupHostName));
            }


            return new PopupAware(() =>
            {

                if (PopupHost.popupAwareMapper.TryGetValue(popupHostName, out PopupHost popupAware))
                {
                    return popupAware;
                }

                throw new ArgumentException($"target PopupHost with the name :{popupHostName} not exists");
            }, popupHostName);
        }
    }

    public interface IPopupAware
    {
        string HostName { get; }
        Task ShowAsync(string message, PopupConfig config = null);
        Task<bool> ConfirmAsync(string message, PopupConfig config = null);
        Task<bool> PopupAsync<Target>(Func<Target> uIElementFunc, PopupConfig config = null) where Target : View, IPopupContent;

    }

    public interface IPopupManager
    {
        IPopupAware Aware(string popupHostName);
        Task ShowAsync(string popupHostName, string message, PopupConfig config = null);
        Task<bool> ConfirmAsync(string popupHostName, string message, PopupConfig config = null);
        Task<bool> PopupAsync<Target>(string popupHostName, Func<Target> uIElementFunc, PopupConfig config = null) where Target : View, IPopupContent;
    }
}
