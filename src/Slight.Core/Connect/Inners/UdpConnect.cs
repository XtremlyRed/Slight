using System;
using System.ComponentModel;
using System.Diagnostics;
using System.Net;
using System.Net.Sockets;
using System.Threading;
using System.Threading.Tasks;


namespace Slight.Core.Connect
{
    /// <summary>
    /// UdpConnect
    /// </summary>
    public class UdpConnect : IUdpConnect
    {
        [EditorBrowsable(EditorBrowsableState.Never)]
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private readonly EndPoint emptyEndPoint = new IPEndPoint(IPAddress.Any, 0);

        [EditorBrowsable(EditorBrowsableState.Never)]
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private readonly ConnectConfiguration connectConfiguration;

        [EditorBrowsable(EditorBrowsableState.Never)]
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private Socket socket;

        [EditorBrowsable(EditorBrowsableState.Never)]
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private AsyncTransferProxy asyncTransferProxy;


        [EditorBrowsable(EditorBrowsableState.Never)]
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private bool isDisposed;


        /// <summary>
        /// create udp connect by  <paramref name="connectConfiguration"/>
        /// </summary>
        /// <param name="connectConfiguration"></param>
        public UdpConnect(ConnectConfiguration connectConfiguration)
        {
            this.connectConfiguration = connectConfiguration;
        }

        /// <summary>
        /// startup udp connect
        /// </summary>
        /// <returns></returns>
        /// <exception cref="ArgumentNullException">if localEndPoint is null</exception>
        /// <exception cref="ArgumentOutOfRangeException">if connectConfiguration.bufferSize <= 0</exception>
        public IMessageTransfer RunAsync()
        {
            if (connectConfiguration.localEndPoint is null)
            {
                throw new ArgumentNullException(nameof(connectConfiguration.localEndPoint));
            }
            if (connectConfiguration.bufferSize <= 0)
            {
                throw new ArgumentOutOfRangeException(nameof(connectConfiguration.bufferSize));
            }

            socket = new Socket(AddressFamily.InterNetwork, SocketType.Dgram, ProtocolType.Udp);
            socket.SetSocketOption(SocketOptionLevel.Socket, SocketOptionName.ReuseAddress, true);
            socket.Bind(connectConfiguration.localEndPoint);
            socket.DontFragment = true;

            asyncTransferProxy = new AsyncTransferProxy(connectConfiguration.bufferSize);

            SocketAsyncEventArgs socketArgs = new();
            socketArgs.SetBuffer(new byte[connectConfiguration.bufferSize], 0, connectConfiguration.bufferSize);
            socketArgs.Completed += EndReceive;
            socketArgs.RemoteEndPoint = emptyEndPoint;
            BeginReceive(socketArgs);

            return new MessageTransfer(socket, connectConfiguration.remoteEndPoint, asyncTransferProxy, ThrowIfDisopsed);
        }


        /// <summary>
        /// BeginReceive
        /// </summary>
        /// <param name="e"></param>
        private void BeginReceive(SocketAsyncEventArgs e)
        {
            ThrowIfDisopsed();

            if (socket.ReceiveFromAsync(e) == false)
            {
                EndReceive(this, e);
            }
        }
        /// <summary>
        /// completed handle
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void EndReceive(object sender, SocketAsyncEventArgs e)
        {
            ThrowIfDisopsed();

            if (e.BytesTransferred > 0 && e.SocketError == SocketError.Success)
            {
                byte[] currentReceviedBuffer = new byte[e.BytesTransferred];

                Buffer.BlockCopy(e.Buffer, 0, currentReceviedBuffer, 0, e.BytesTransferred);

                MessageTransfer tran = new(socket, connectConfiguration.remoteEndPoint, asyncTransferProxy, ThrowIfDisopsed);

                Task.Factory.StartNew(() =>
                {
                    try
                    {
                        connectConfiguration.recevieCallback?.Invoke(tran, currentReceviedBuffer);

                        currentReceviedBuffer = null;
                    }
                    catch (Exception e)
                    {
                        connectConfiguration.errorCallback?.Invoke(e);
                    }
                }, CancellationToken.None, TaskCreationOptions.DenyChildAttach, TaskScheduler.Default);

            }

            BeginReceive(e);
        }

        private void ThrowIfDisopsed()
        {
            if (isDisposed)
            {
                throw new ObjectDisposedException(nameof(UdpConnect));
            }
        }


        /// <summary>
        /// Dispose
        /// </summary>
        public void Dispose()
        {
            isDisposed = true;
            socket?.Close();
            socket?.Dispose();
            socket = null;
        }
    }
}
