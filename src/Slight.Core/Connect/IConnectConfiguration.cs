using System;
using System.ComponentModel;
using System.Diagnostics;
using System.Net;

namespace Slight.Core
{
    /// <summary>
    /// connect config 
    /// </summary>
    public interface IConnectConfiguration
    {
        /// <summary>
        /// bufer size
        /// </summary>
        /// <param name="bufferSize"></param>
        /// <returns></returns>
        IConnectConfiguration UseBufferSize(int bufferSize);

        /// <summary>
        /// local endpoint ip:127.0.0.1
        /// </summary>
        /// <param name="port"></param>
        /// <returns></returns>
        IConnectConfiguration UseLocalEndPoint(int port);

        /// <summary>
        /// remote endpoint ip:127.0.0.1
        /// </summary>
        /// <param name="port"></param>
        /// <returns></returns>
        IConnectConfiguration UseRemoteEndPoint(int port);

        /// <summary>
        /// local endpoint
        /// </summary>
        /// <param name="localEndPoint"></param>
        /// <returns></returns>
        IConnectConfiguration UseLocalEndPoint(EndPoint localEndPoint);

        /// <summary>
        /// remote endpoint
        /// </summary>
        /// <param name="remoteEndPoint"></param>
        /// <returns></returns>
        IConnectConfiguration UseRemoteEndPoint(EndPoint remoteEndPoint);

        /// <summary>
        /// execute when has exception
        /// </summary>
        /// <param name="errorCallback"></param>
        /// <returns></returns>
        IConnectConfiguration UseErrorCallback(Action<Exception> errorCallback);

        /// <summary>
        /// execute when recevied data 
        /// </summary>
        /// <param name="recevieCallback"></param>
        /// <returns></returns>
        IConnectConfiguration UseRecevieCallback(Action<IMessageTransfer, byte[]> recevieCallback);

        /// <summary>
        /// using udp
        /// </summary>
        /// <returns></returns>
        IUdpConnect UseUdpConnect();

        /// <summary>
        /// using tcp
        /// </summary>
        /// <returns></returns>
        ITcpConnect UseTcpConnect();
    }
}
