using System;
using System.Threading.Tasks;
using System.Windows;

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
             where Target : UIElement, IPopupContent
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
                bool flag1 = PopupHost.popupAwareMapper.TryGetValue(popupHostName, out PopupHost.PopupHostStatus popupAware);

                if (flag1 && popupAware?.PopupHost is IPopupAware popup)
                {
                    return popup;
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
        Task<bool> PopupAsync<Target>(Func<Target> uIElementFunc, PopupConfig config = null) where Target : UIElement, IPopupContent;

    }

    /// <summary>
    /// popup mananger
    /// </summary>
    public interface IPopupManager
    {

        /// <summary>
        /// aware popup host by popup host name
        /// </summary>
        /// <param name="popupHostName"></param>
        /// <returns></returns>
        IPopupAware Aware(string popupHostName);

        /// <summary>
        /// popup message
        /// </summary>
        /// <param name="popupHostName"></param>
        /// <param name="message"></param>
        /// <param name="config"></param>
        /// <returns></returns>
        Task ShowAsync(string popupHostName, string message, PopupConfig config = null);

        /// <summary>
        /// popup comfirm message
        /// </summary>
        /// <param name="popupHostName"></param>
        /// <param name="message"></param>
        /// <param name="config"></param>
        /// <returns></returns>
        Task<bool> ConfirmAsync(string popupHostName, string message, PopupConfig config = null);

        /// <summary>
        /// popup  <see cref="IPopupContent"/> conttrol 
        /// </summary>
        /// <typeparam name="Target"></typeparam>
        /// <param name="popupHostName"></param>
        /// <param name="uIElementFunc"></param>
        /// <param name="config"></param>
        /// <returns></returns>
        Task<bool> PopupAsync<Target>(string popupHostName, Func<Target> uIElementFunc, PopupConfig config = null) where Target : UIElement, IPopupContent;
    }

    /// <summary>
    ///  Popup Config
    /// </summary>
    public class PopupConfig
    {
        internal static Size DefaultPopupVisualSize { get; private set; } = new Size(500, 300);

        /// <summary>
        /// default cancen button text to display
        /// </summary>
        public static string DefaultCancenButtonText { get; private set; } = "Cancel";

        /// <summary>
        /// default confirm button text to display
        /// </summary>
        public static string DefaultConfirmButtonText { get; private set; } = "Confirm";

        /// <summary>
        /// default title to display
        /// </summary>
        public static string DefaultTitle { get; private set; } = "Tips";

        /// <summary>
        /// title
        /// </summary>
        public string Title { get; set; }

        /// <summary>
        /// popup visual size
        /// </summary>
        public Size PopupVisualSize { get; set; } = new Size(double.NaN, double.NaN);

        /// <summary>
        /// cureate from popup title
        /// </summary>
        /// <param name="popupTitle"></param>
        public static implicit operator PopupConfig(string popupTitle)
        {
            return new PopupConfig()
            {
                Title = popupTitle,
            };
        }

        /// <summary>
        /// create from popup size
        /// </summary>
        /// <param name="popupVisualSize"></param>
        public static implicit operator PopupConfig(Size popupVisualSize)
        {
            return new PopupConfig()
            {
                PopupVisualSize = popupVisualSize,
            };
        }

        /// <summary>
        /// update default text to displau 
        /// </summary>
        /// <param name="defaultMessageTitle"></param>
        /// <param name="cancelButtonText"></param>
        /// <param name="confirmButtonText"></param>
        /// <exception cref="ArgumentNullException"></exception>
        public static void UpdateDefaultText(string defaultMessageTitle, string cancelButtonText, string confirmButtonText)
        {
            if (string.IsNullOrWhiteSpace(defaultMessageTitle))
            {
                throw new ArgumentNullException(nameof(defaultMessageTitle));
            }
            if (string.IsNullOrWhiteSpace(cancelButtonText))
            {
                throw new ArgumentNullException(nameof(cancelButtonText));
            }
            if (string.IsNullOrWhiteSpace(confirmButtonText))
            {
                throw new ArgumentNullException(nameof(confirmButtonText));
            }
            DefaultTitle = defaultMessageTitle;
            DefaultCancenButtonText = cancelButtonText;
            DefaultConfirmButtonText = confirmButtonText;
        }
    }
}
