using System;
using System.ComponentModel;
using System.Diagnostics;
using System.Net;
using System.Net.Sockets;

namespace Slight.Core.Connect
{

    /// <summary>
    /// message send proxy
    /// </summary>
    [DebuggerDisplay("{RemoteEndPoint}")]
    public class MessageTransfer : IMessageTransfer
    {
        /// <summary>
        /// remote end point
        /// </summary>
        public EndPoint RemoteEndPoint { get; }

        [EditorBrowsable(EditorBrowsableState.Never)]
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private readonly AsyncTransferProxy asyncSendPool;

        [EditorBrowsable(EditorBrowsableState.Never)]
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private readonly Action disposeChecker;

        [EditorBrowsable(EditorBrowsableState.Never)]
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private readonly Socket socket;

        internal MessageTransfer(Socket socket, AsyncTransferProxy asyncSendPool, Action disposeChecker)
        {
            this.socket = socket;
            RemoteEndPoint = socket?.RemoteEndPoint;
            this.asyncSendPool = asyncSendPool;
            this.disposeChecker = disposeChecker;
        }

        internal MessageTransfer(Socket socket, EndPoint remoteEndPoint, AsyncTransferProxy asyncSendPool, Action disposeChecker)
        {
            this.socket = socket;
            RemoteEndPoint = remoteEndPoint;
            this.asyncSendPool = asyncSendPool;
            this.disposeChecker = disposeChecker;
        }

        /// <summary>
        /// message sender
        /// </summary>
        /// <param name="buffer"></param>
        /// <param name="offset"></param>
        /// <param name="length"></param>
        /// <returns></returns>
        /// <exception cref="ArgumentNullException"></exception>
        /// <exception cref="InvalidOperationException"></exception>
        public IMessageTransfer Transfer(byte[] buffer, int offset, int length)
        {
            disposeChecker?.Invoke();
            if (RemoteEndPoint is null)
            {
                throw new ArgumentNullException(nameof(RemoteEndPoint));
            }
            if (asyncSendPool is null)
            {
                throw new InvalidOperationException();
            }
            asyncSendPool.SendAsync(socket, socket?.RemoteEndPoint, buffer, offset, length);
            return this;
        }
    }
}
