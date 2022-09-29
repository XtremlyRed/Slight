using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Net;
using System.Net.Sockets;
using System.Threading;
using System.Threading.Tasks;

namespace Slight.Core.Connect
{
    /// <summary>
    /// <see cref="TcpConnect"/>
    /// </summary>
    public class TcpConnect : ITcpConnect, ITcpServer
    {
        [EditorBrowsable(EditorBrowsableState.Never)]
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private const int Int32Size = sizeof(int);

        [EditorBrowsable(EditorBrowsableState.Never)]
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private readonly EndPoint emptyEndPoint = new IPEndPoint(IPAddress.Any, 0);

        [EditorBrowsable(EditorBrowsableState.Never)]
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private readonly ConnectConfiguration connectConfiguration;

        [EditorBrowsable(EditorBrowsableState.Never)]
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private AsyncTransferProxy asyncSendPool;

        [EditorBrowsable(EditorBrowsableState.Never)]
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private bool isDisposed;

        [EditorBrowsable(EditorBrowsableState.Never)]
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private int backlog = -1;

        [EditorBrowsable(EditorBrowsableState.Never)]
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private int receviePoolSize = -1;

        [EditorBrowsable(EditorBrowsableState.Never)]
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private Action<IMessageTransfer> acceptCallback;

        [EditorBrowsable(EditorBrowsableState.Never)]
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private readonly Dictionary<Socket, AsyncUserToken> socketBufferPool = new();

        /// <summary>
        /// create tcp connect by  <paramref name="connectConfiguration"/>
        /// </summary>
        /// <param name="connectConfiguration"></param>
        public TcpConnect(ConnectConfiguration connectConfiguration)
        {
            this.connectConfiguration = connectConfiguration;
        }
        void ITcpServer.RunAsync()
        {
            Socket socket = SocketBuilder();

            if (backlog <= 0)
            {
                throw new ArgumentOutOfRangeException(nameof(backlog));
            }
            socket.Listen(backlog);
            AcceptAsync(socket);
        }


        /// <summary>
        /// startup tcp connect
        /// </summary>
        /// <returns></returns>
        /// <exception cref="ArgumentNullException"></exception>
        IMessageTransfer ITcpConnect.RunAsync()
        {
            Socket socket = SocketBuilder();

            if (connectConfiguration.remoteEndPoint is null)
            {
                throw new ArgumentNullException(nameof(connectConfiguration.remoteEndPoint));
            }

            SocketAsyncEventArgs socketConnectArgs = new()
            {
                RemoteEndPoint = connectConfiguration.remoteEndPoint,
                UserToken = socket
            };

            bool connectResult = socket.ConnectAsync(socketConnectArgs);

            BeginRecevieMessage(socket);

            return new MessageTransfer(socket, asyncSendPool, ThrowIfDisopsed);
        }


        private Socket SocketBuilder()
        {
            if (connectConfiguration.localEndPoint is null)
            {
                throw new ArgumentNullException(nameof(connectConfiguration.localEndPoint));
            }

            if (connectConfiguration.bufferSize <= 0)
            {
                throw new ArgumentOutOfRangeException(nameof(connectConfiguration.bufferSize));
            }

            if (receviePoolSize < 0)
            {
                receviePoolSize = connectConfiguration.bufferSize;
            }

            asyncSendPool = new AsyncTransferProxy(connectConfiguration.bufferSize);

            Socket socket = new(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            socket.SetSocketOption(SocketOptionLevel.Socket, SocketOptionName.ReuseAddress, true);
            socket.Bind(connectConfiguration.localEndPoint);
            socket.DontFragment = true;

            return socket;
        }

        /// <summary>
        /// backlog
        /// </summary>
        /// <param name="backlog"></param>
        /// <returns></returns>
        public ITcpServer UseListen(int backlog)
        {
            this.backlog = backlog;
            return this;
        }

        /// <summary>
        /// acceptCallback
        /// </summary>
        /// <param name="acceptCallback"></param>
        /// <returns></returns>
        public ITcpServer UseAccept(Action<IMessageTransfer> acceptCallback)
        {
            this.acceptCallback = acceptCallback;
            return this;
        }

        ITcpServer ITcpServer.UseReceviePoolSize(int receviePoolSize)
        {
            this.receviePoolSize = receviePoolSize;
            return this;
        }


        ITcpConnect ITcpConnect.UseReceviePoolSize(int receviePoolSize)
        {
            this.receviePoolSize = receviePoolSize;
            return this;
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
            asyncSendPool?.Dispose();
            asyncSendPool = null;
            socketBufferPool?.ForEach(i =>
            {
                try
                {
                    i.Value.Dispose();
                    i.Key.Close();
                    i.Key.Dispose();
                }
                catch
                {
                }
            });
        }


        #region  accept


        private void AcceptAsync(Socket socket)
        {
            SocketAsyncEventArgs socketAsyncEventArgs = new();
            socketAsyncEventArgs.Completed += AcceptAsyncEventArgs_Completed;
            socketAsyncEventArgs.UserToken = socket;
            if (!socket.AcceptAsync(socketAsyncEventArgs))
            {
                AcceptAsyncEventArgs_Completed(null, socketAsyncEventArgs);
            }
        }

        private void AcceptAsyncEventArgs_Completed(object sender, SocketAsyncEventArgs acceptEventArgs)
        {
            acceptEventArgs.Completed -= AcceptAsyncEventArgs_Completed;

            BeginRecevieMessage(acceptEventArgs.AcceptSocket);

            Task.Factory.StartNew(() =>
            {
                if (acceptCallback != null)
                {
                    MessageTransfer messageTransfer = new(acceptEventArgs.AcceptSocket, asyncSendPool, ThrowIfDisopsed);
                    acceptCallback.Invoke(messageTransfer);
                }
            }, CancellationToken.None, TaskCreationOptions.DenyChildAttach, TaskScheduler.Default);

            if (acceptEventArgs.SocketError == SocketError.OperationAborted)
            {
                return;
            }

            AcceptAsync(acceptEventArgs.UserToken as Socket);
        }

        private void BeginRecevieMessage(Socket socket)
        {
            if (socketBufferPool.TryGetValue(socket, out AsyncUserToken userToken) == false)
            {
                userToken = new AsyncUserToken
                {
                    BufferPool = new AsyncUserToken.ByteAnnularPool(receviePoolSize),
                    PacketLength = 0,
                    Socket = socket,
                    ConnectTime = DateTime.Now
                };
                if (socket.RemoteEndPoint is IPEndPoint endPoint)
                {
                    userToken.IpEndPoint = endPoint;
                }
                socketBufferPool[socket] = userToken;
            }

            SocketAsyncEventArgs recevieEventArgs = asyncSendPool.Rent();

            recevieEventArgs.UserToken = userToken;
            recevieEventArgs.Completed += IO_Completed;

            if (socket.ReceiveAsync(recevieEventArgs) == false)
            {
                RecevieMessage(recevieEventArgs);
            }
        }

        private void RecevieMessage(SocketAsyncEventArgs recevieEventArgs)
        {
            if (recevieEventArgs.UserToken is not AsyncUserToken userToken)
            {
                return;
            }

            recevieEventArgs.Completed -= IO_Completed;

            if (recevieEventArgs.BytesTransferred > 0 && recevieEventArgs.SocketError == SocketError.Success)
            {
                userToken.BufferPool.Write(recevieEventArgs.Buffer, recevieEventArgs.Offset, recevieEventArgs.BytesTransferred);



                do
                {
                    int canReadLength = userToken.BufferPool.CanReadLength;

                    if (canReadLength == 0)
                    {
                        break;
                    }

                    if (userToken.PacketLength == 0)
                    {
                        if (canReadLength < Int32Size)
                        {
                            break;
                        }

                        userToken.BufferPool.Read(userToken.PacketLengthBuffer, 0, userToken.PacketLengthBuffer.Length);

                        canReadLength -= Int32Size;

                        userToken.PacketLength = BitConverter.ToInt32(userToken.PacketLengthBuffer, 0);
                         
                        userToken.PacketBuffer = new byte[userToken.PacketLength];
                         
                    }

                    if (canReadLength >= 0)
                    {
                        int plusBufferLength = userToken.PacketLength - userToken.PacketOffset;
                        if (plusBufferLength <= canReadLength)
                        {
                            userToken.BufferPool.Read(userToken.PacketBuffer, userToken.PacketOffset, plusBufferLength);
                            userToken.PacketOffset += plusBufferLength;
                        }
                        else
                        {
                            userToken.BufferPool.Read(userToken.PacketBuffer, userToken.PacketOffset, canReadLength);
                            userToken.PacketOffset += canReadLength;
                        }

                        if (userToken.PacketOffset != userToken.PacketLength)
                        {
                            continue;
                        }

                        byte[] packetBuffer = userToken.PacketBuffer;

                        userToken.PacketLength = userToken.PacketOffset = 0;

                        Task.Factory.StartNew(() =>
                        {
                            try
                            {


                                MessageTransfer transfer = new(userToken.Socket, asyncSendPool, ThrowIfDisopsed);

                                connectConfiguration.recevieCallback?.Invoke(transfer, packetBuffer);
                            }
                            catch (Exception ex)
                            {
                                connectConfiguration.errorCallback?.Invoke(ex);
                            }
                        }, CancellationToken.None, TaskCreationOptions.DenyChildAttach, TaskScheduler.Default);

                        continue;
                    }

                    break;

                } while (true);

                BeginRecevieMessage(userToken.Socket);

                asyncSendPool.Return(recevieEventArgs);

            }
        }

        private void IO_Completed(object sender, SocketAsyncEventArgs e)
        {
            // determine which type of operation just completed and call the associated handler  
            switch (e.LastOperation)
            {
                case SocketAsyncOperation.Receive:
                    RecevieMessage(e);
                    break;
                case SocketAsyncOperation.Send:
                    ProcessSend(e);
                    break;
                default:
                    throw new ArgumentException("The last operation completed on the socket was not a receive or send");
            }
        }

        private void ProcessSend(SocketAsyncEventArgs socketEventArys)
        {
            if (socketEventArys.SocketError == SocketError.Success)
            {
                AsyncUserToken asyncToken = (AsyncUserToken)socketEventArys.UserToken;
                bool willRaiseEvent = asyncToken.Socket.ReceiveAsync(socketEventArys);
                if (!willRaiseEvent)
                {
                    RecevieMessage(socketEventArys);
                }

                return;
            }

            socketEventArys.Completed -= IO_Completed;
            if (socketEventArys.UserToken is AsyncUserToken token && socketBufferPool.ContainsKey(token.Socket))
            {
                socketBufferPool.Remove(token.Socket);
            }
            CloseClientSocket(socketEventArys);

        }

        //关闭客户端  
        private void CloseClientSocket(SocketAsyncEventArgs socketEventArys)
        {
            AsyncUserToken token = socketEventArys.UserToken as AsyncUserToken;

            try
            {
                token.Socket.Shutdown(SocketShutdown.Send);
            }
            catch (Exception)
            {

            }
            token.Socket.Close();

            socketEventArys.UserToken = null;
        }



        #endregion
    }
}
