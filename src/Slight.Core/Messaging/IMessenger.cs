using System;
using System.Threading.Tasks;

namespace Slight.Core
{
    /// <summary>
    ///  <see cref="IMessenger"/>  interface 
    /// </summary>
    public interface IMessenger
    {
        /// <summary>
        /// Unregister all message callback from subscriber
        /// </summary>
        /// <param name="subscriber"></param>
        void UnregisterAll(object subscriber);

        /// <summary>
        /// Unregister all message callback from subscriber by token
        /// </summary> 
        /// <param name="unregisterToken"></param>
        void Unregister(string unregisterToken);

        /// <summary>
        ///  execute a message callback  by token and other message parameters
        /// </summary>
        /// <param name="publishToken"></param>
        /// <param name="messengerParamters"></param>
        void Publish(string publishToken, params object[] messengerParamters);

        /// <summary>
        ///  async execute a message callback  by token and other message parameters
        /// </summary>
        /// <param name="publishToken"></param>
        /// <param name="messengerParamters"></param>
        /// <returns></returns>
        Task PublishAsync(string publishToken, params object[] messengerParamters);

        /// <summary>
        ///  execute a message callback  by token and other message parameters
        /// </summary>
        /// <typeparam name="TResult"></typeparam>
        /// <param name="publishToken"></param>
        /// <param name="messengerParamters"></param>
        /// <returns><typeparamref name="TResult"/></returns>
        TResult Publish<TResult>(string publishToken, params object[] messengerParamters);

        /// <summary>
        ///  async execute a message callback  by token and other message parameters
        /// </summary>
        /// <typeparam name="TResult"></typeparam>
        /// <param name="publishToken"></param>
        /// <param name="messengerParamters"></param>
        /// <returns><typeparamref name="TResult"/></returns>
        Task<TResult> PublishAsync<TResult>(string publishToken, params object[] messengerParamters);


        #region 0

        void Subscribe<TResult>(object subscriber, string token, Func<TResult> callbackHandler);

        void Subscribe(object subscriber, string token, Action callbackHandler);

        #endregion


        #region 1

        void Subscribe<TMessage, TResult>(object subscriber, string token, Func<TMessage, TResult> callbackHandler);

        void Subscribe<TMessage>(object subscriber, string token, Action<TMessage> callbackHandler);

        #endregion

        #region 2

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="TMessage1"></typeparam>
        /// <typeparam name="TMessage2"></typeparam>
        /// <typeparam name="TResult"></typeparam>
        /// <param name="subscriber"></param>
        /// <param name="token"></param>
        /// <param name="callbackHandler"></param>
        /// <returns></returns>
        void Subscribe<TMessage1, TMessage2, TResult>(object subscriber, string token,
            Func<TMessage1, TMessage2, TResult> callbackHandler);

        void Subscribe<TMessage1, TMessage2>(object subscriber, string token,
            Action<TMessage1, TMessage2> callbackHandler);

        #endregion


        #region 3

        void Subscribe<TMessage1, TMessage2, TMessage3, TResult>(object subscriber, string token,
            Func<TMessage1, TMessage2, TMessage3, TResult> callbackHandler);

        void Subscribe<TMessage1, TMessage2, TMessage3>(object subscriber, string token,
            Action<TMessage1, TMessage2, TMessage3> callbackHandler);

        #endregion


        #region 4

        void Subscribe<TMessage1, TMessage2, TMessage3, TMessage4, TResult>(object subscriber, string token,
            Func<TMessage1, TMessage2, TMessage3, TMessage4, TResult> callbackHandler);

        void Subscribe<TMessage1, TMessage2, TMessage3, TMessage4>(object subscriber, string token,
            Action<TMessage1, TMessage2, TMessage3, TMessage4> callbackHandler
             );

        #endregion

        #region 5

        void Subscribe<TMessage1, TMessage2, TMessage3, TMessage4, TMessage5, TResult>(object subscriber, string token,
            Func<TMessage1, TMessage2, TMessage3, TMessage4, TMessage5, TResult> callbackHandler);

        void Subscribe<TMessage1, TMessage2, TMessage3, TMessage4, TMessage5>(object subscriber, string token,
            Action<TMessage1, TMessage2, TMessage3, TMessage4, TMessage5> callbackHandler
             );

        #endregion


        #region 6

        void Subscribe<TMessage1, TMessage2, TMessage3, TMessage4, TMessage5, TMessage6, TResult>(object subscriber,
            string token,
            Func<TMessage1, TMessage2, TMessage3, TMessage4, TMessage5, TMessage6, TResult> callbackHandler);

        void Subscribe<TMessage1, TMessage2, TMessage3, TMessage4, TMessage5, TMessage6>(object subscriber, string token,
            Action<TMessage1, TMessage2, TMessage3, TMessage4, TMessage5, TMessage6> callbackHandler
             );

        #endregion


        #region 7

        void Subscribe<TMessage1, TMessage2, TMessage3, TMessage4, TMessage5, TMessage6, TMessage7, TResult>(
            object subscriber, string token,
            Func<TMessage1, TMessage2, TMessage3, TMessage4, TMessage5, TMessage6, TMessage7, TResult> callbackHandler);

        void Subscribe<TMessage1, TMessage2, TMessage3, TMessage4, TMessage5, TMessage6, TMessage7>(object subscriber,
            string token,
            Action<TMessage1, TMessage2, TMessage3, TMessage4, TMessage5, TMessage6, TMessage7> callbackHandler
             );

        #endregion


        #region 8

        void Subscribe<TMessage1, TMessage2, TMessage3, TMessage4, TMessage5, TMessage6, TMessage7, TMessage8, TResult>(
            object subscriber, string token,
            Func<TMessage1, TMessage2, TMessage3, TMessage4, TMessage5, TMessage6, TMessage7, TMessage8, TResult>
                callbackHandler);

        void Subscribe<TMessage1, TMessage2, TMessage3, TMessage4, TMessage5, TMessage6, TMessage7, TMessage8>(
            object subscriber, string token,
            Action<TMessage1, TMessage2, TMessage3, TMessage4, TMessage5, TMessage6, TMessage7, TMessage8>
                callbackHandler);

        #endregion


        #region 9

        void Subscribe<TMessage1, TMessage2, TMessage3, TMessage4, TMessage5, TMessage6, TMessage7, TMessage8, TMessage9,
            TResult>(object subscriber, string token,
            Func<TMessage1, TMessage2, TMessage3, TMessage4, TMessage5, TMessage6, TMessage7, TMessage8, TMessage9,
                TResult> callbackHandler);

        void Subscribe<TMessage1, TMessage2, TMessage3, TMessage4, TMessage5, TMessage6, TMessage7, TMessage8,
            TMessage9>(object subscriber, string token,
            Action<TMessage1, TMessage2, TMessage3, TMessage4, TMessage5, TMessage6, TMessage7, TMessage8, TMessage9>
                callbackHandler);

        #endregion


        #region 10

        void Subscribe<TMessage1, TMessage2, TMessage3, TMessage4, TMessage5, TMessage6, TMessage7, TMessage8, TMessage9,
            TMessage10, TResult>(object subscriber, string token,
            Func<TMessage1, TMessage2, TMessage3, TMessage4, TMessage5, TMessage6, TMessage7, TMessage8, TMessage9,
                TMessage10, TResult> callbackHandler);

        void Subscribe<TMessage1, TMessage2, TMessage3, TMessage4, TMessage5, TMessage6, TMessage7, TMessage8, TMessage9,
            TMessage10>(object subscriber, string token,
            Action<TMessage1, TMessage2, TMessage3, TMessage4, TMessage5, TMessage6, TMessage7, TMessage8, TMessage9,
                TMessage10> callbackHandler);

        #endregion


        #region 11

        void Subscribe<TMessage1, TMessage2, TMessage3, TMessage4, TMessage5, TMessage6, TMessage7, TMessage8, TMessage9,
            TMessage10, TMessage11, TResult>(object subscriber, string token,
            Func<TMessage1, TMessage2, TMessage3, TMessage4, TMessage5, TMessage6, TMessage7, TMessage8, TMessage9,
                TMessage10, TMessage11, TResult> callbackHandler);

        void Subscribe<TMessage1, TMessage2, TMessage3, TMessage4, TMessage5, TMessage6, TMessage7, TMessage8, TMessage9,
            TMessage10, TMessage11>(object subscriber, string token,
            Action<TMessage1, TMessage2, TMessage3, TMessage4, TMessage5, TMessage6, TMessage7, TMessage8, TMessage9,
                TMessage10, TMessage11> callbackHandler);

        #endregion
    }
}
