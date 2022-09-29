using System;

namespace Slight.Core
{
    /// <summary>
    /// <see cref="ITcpConnect"/>
    /// </summary>
    public interface ITcpConnect : ITcpServer
    {

        /// <summary>
        /// recevie pool size
        /// </summary>
        /// <param name="receviePoolSize"></param>
        /// <returns></returns>
        new ITcpConnect UseReceviePoolSize(int receviePoolSize);

        /// <summary>
        /// startup tcp connect
        /// </summary>
        /// <returns></returns>
        new IMessageTransfer RunAsync();


    }

    /// <summary>
    /// <see cref="ITcpServer"/>
    /// </summary>
    public interface ITcpServer
    {

        /// <summary>
        /// pool size
        /// </summary>
        /// <param name="receviePoolSize"></param>
        /// <returns></returns>
        ITcpServer UseReceviePoolSize(int receviePoolSize);

        /// <summary>
        /// accept socket 
        /// </summary>
        /// <param name="acceptCallback"></param>
        /// <returns></returns>
        ITcpServer UseAccept(Action<IMessageTransfer> acceptCallback);

        /// <summary>
        /// set  backlog
        /// </summary>
        /// <param name="backlog"></param>
        /// <returns></returns>
        ITcpServer UseListen(int backlog);
        /// <summary>
        /// startup tcp connect
        /// </summary>
        /// <returns></returns>
        void RunAsync();
    }
}
