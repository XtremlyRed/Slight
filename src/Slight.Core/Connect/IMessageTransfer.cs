using System.Net;

namespace Slight.Core
{
    /// <summary>
    /// message sender
    /// </summary>
    public interface IMessageTransfer
    {
        /// <summary>
        /// remote endpoint
        /// </summary>
        EndPoint RemoteEndPoint { get; }

        /// <summary>
        /// send message to remote endpoint
        /// </summary>
        /// <param name="buffer"></param>
        /// <param name="offset"></param>
        /// <param name="length"></param>
        /// <returns></returns>
        IMessageTransfer Transfer(byte[] buffer, int offset, int length);
    }
}
