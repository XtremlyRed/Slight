using System;
using System.Collections.Concurrent;
using System.ComponentModel;
using System.Diagnostics;
using System.Net;
using System.Net.Sockets;

namespace Slight.Core.Connect
{
    /// <summary>
    /// async send <see cref="SocketAsyncEventArgs"/> proxy
    /// </summary>
    internal class AsyncTransferProxy : IDisposable
    {
        [EditorBrowsable(EditorBrowsableState.Never)]
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private readonly int bufferSize;

        [EditorBrowsable(EditorBrowsableState.Never)]
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private ConcurrentStack<SocketAsyncEventArgs> socketArgsAutoStack = new();

        [EditorBrowsable(EditorBrowsableState.Never)]
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private ConcurrentStack<SocketAsyncEventArgs> socketArgsManualStack = new();
        /// <summary>
        /// new
        /// </summary>
        /// <param name="bufferSize"></param> 
        public AsyncTransferProxy(int bufferSize)
        {
            this.bufferSize = bufferSize;
        }

        /// <summary>
        /// send completed handle
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void SendCompleted(object sender, SocketAsyncEventArgs e)
        {
            socketArgsAutoStack.Push(e);
        }


        #region Public Methods

        /// <summary>
        /// Popup
        /// </summary>
        /// <returns></returns>
        public SocketAsyncEventArgs Rent()
        {
            if (socketArgsManualStack.TryPop(out SocketAsyncEventArgs e))
            {
                return e;
            }
            e = new SocketAsyncEventArgs(); 
            e.SetBuffer(new byte[bufferSize], 0, bufferSize);
            return e;
        }


        /// <summary>
        /// Popup
        /// </summary>
        /// <returns></returns>
        private SocketAsyncEventArgs AutoRent()
        {
            if (socketArgsAutoStack.TryPop(out SocketAsyncEventArgs e))
            {
                return e;
            }

            e = new SocketAsyncEventArgs();
            e.SetBuffer(new byte[bufferSize], 0, bufferSize);
            e.Completed += SendCompleted;
            return e;
        }
        /// <summary>
        /// release
        /// </summary>
        /// <param name="e"></param>
        public void Return(SocketAsyncEventArgs e)
        {
            socketArgsManualStack.Push(e);
        }
        /// <summary>
        /// sned async
        /// </summary>
        /// <param name="socket"></param>
        /// <param name="buffer"></param>
        /// <param name="offset"></param>
        /// <param name="length"></param>
        /// <exception cref="ArgumentNullException">socket is null</exception>
        /// <exception cref="ArgumentNullException">buffer is null or empty</exception>
        /// <exception cref="ArgumentOutOfRangeException">buffer length > messageBufferSize</exception>
        public void SendAsync(Socket socket, byte[] buffer, int offset, int length)
        {
            if (buffer == null)
            {
                throw new ArgumentNullException(nameof(buffer));
            }

            if (buffer.Length - offset < length || length > bufferSize)
            {
                throw new ArgumentOutOfRangeException(nameof(length));
            }

            SocketAsyncEventArgs e = AutoRent();
            e.RemoteEndPoint = socket.RemoteEndPoint ?? throw new ArgumentNullException("endPoint");

            if (socket.ProtocolType == ProtocolType.Tcp)
            {
                var lengthBytes = BitConverter.GetBytes(length);
                Buffer.BlockCopy(lengthBytes, 0, e.Buffer, 0, lengthBytes.Length);
                Buffer.BlockCopy(buffer, offset, e.Buffer, lengthBytes.Length, length);
                e.SetBuffer(0, length + lengthBytes.Length);
            }
            else
            {
                Buffer.BlockCopy(buffer, offset, e.Buffer, 0, length);
                e.SetBuffer(0, length);
            }

            if (socket.SendToAsync(e) == false)
            {
                socketArgsAutoStack.Push(e);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="socket"></param>
        /// <param name="endPoint"></param>
        /// <param name="buffer"></param>
        /// <param name="offset"></param>
        /// <param name="length"></param>
        /// <exception cref="ArgumentNullException"></exception>
        /// <exception cref="ArgumentOutOfRangeException"></exception>
        public void SendAsync(Socket socket, EndPoint endPoint, byte[] buffer, int offset, int length)
        {
            if (buffer == null)
            {
                throw new ArgumentNullException(nameof(buffer));
            }

            if (buffer.Length - offset < length || length > bufferSize)
            {
                throw new ArgumentOutOfRangeException(nameof(length));
            }

            SocketAsyncEventArgs e = AutoRent();
            e.RemoteEndPoint = endPoint ?? throw new ArgumentNullException("endPoint");

            if (socket.ProtocolType == ProtocolType.Tcp)
            {
                var lengthBytes = BitConverter.GetBytes(length);
                Buffer.BlockCopy(lengthBytes, 0, e.Buffer, 0, lengthBytes.Length);
                Buffer.BlockCopy(buffer, offset, e.Buffer, lengthBytes.Length, length);
                e.SetBuffer(0, length + lengthBytes.Length);
            }
            else
            {
                Buffer.BlockCopy(buffer, offset, e.Buffer, 0, length);
                e.SetBuffer(0, length);
            }

            if (socket.SendToAsync(e) == false)
            {
                socketArgsAutoStack.Push(e);
            }
        }

        /// <summary>
        /// Dispose
        /// </summary>
        public void Dispose()
        {
            socketArgsAutoStack?.ForEach(x => x.Completed -= SendCompleted);
            socketArgsAutoStack?.Clear();
            socketArgsAutoStack = null;

            socketArgsManualStack?.Clear();
            socketArgsManualStack = null;
        }
        #endregion
    }
}
