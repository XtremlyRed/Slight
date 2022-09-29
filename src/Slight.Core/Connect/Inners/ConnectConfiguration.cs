using System;
using System.ComponentModel;
using System.Diagnostics;
using System.Net;

namespace Slight.Core.Connect
{
    /// <summary>
    /// connect config 
    /// </summary>
    public class ConnectConfiguration : IConnectConfiguration
    {
        [EditorBrowsable(EditorBrowsableState.Never)]
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        internal int bufferSize;

        [EditorBrowsable(EditorBrowsableState.Never)]
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        internal Action<Exception> errorCallback;

        [EditorBrowsable(EditorBrowsableState.Never)]
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        internal EndPoint localEndPoint;

        [EditorBrowsable(EditorBrowsableState.Never)]
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        internal Action<IMessageTransfer, byte[]> recevieCallback;

        [EditorBrowsable(EditorBrowsableState.Never)]
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        internal EndPoint remoteEndPoint;

        IConnectConfiguration IConnectConfiguration.UseBufferSize(int bufferSize)
        {
            this.bufferSize = bufferSize;
            return this;
        }

        IConnectConfiguration IConnectConfiguration.UseErrorCallback(Action<Exception> errorCallback)
        {
            this.errorCallback = errorCallback;
            return this;
        }

        IConnectConfiguration IConnectConfiguration.UseLocalEndPoint(EndPoint localEndPoint)
        {

            this.localEndPoint = localEndPoint;
            return this;
        }

        IConnectConfiguration IConnectConfiguration.UseLocalEndPoint(int port)
        {
            localEndPoint = new IPEndPoint(IPAddress.Parse("127.0.0.1"), port);
            return this;
        }
        IConnectConfiguration IConnectConfiguration.UseRemoteEndPoint(int port)
        {
            remoteEndPoint = new IPEndPoint(IPAddress.Parse("127.0.0.1"), port);
            return this;
        }
        IConnectConfiguration IConnectConfiguration.UseRecevieCallback(Action<IMessageTransfer, byte[]> recevieCallback)
        {
            this.recevieCallback = recevieCallback;
            return this;
        }

        IConnectConfiguration IConnectConfiguration.UseRemoteEndPoint(EndPoint remoteEndPoint)
        {
            this.remoteEndPoint = remoteEndPoint;
            return this;
        }


        ITcpConnect IConnectConfiguration.UseTcpConnect()
        {
            return new TcpConnect(Copy());
        }

        IUdpConnect IConnectConfiguration.UseUdpConnect()
        {
            return new UdpConnect(Copy());
        }

        private ConnectConfiguration Copy()
        {
            return new ConnectConfiguration()
            {
                bufferSize = bufferSize,
                errorCallback = errorCallback,
                localEndPoint = localEndPoint,
                recevieCallback = recevieCallback,
                remoteEndPoint = remoteEndPoint
            };
        }
    }
}
