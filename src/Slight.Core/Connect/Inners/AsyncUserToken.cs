using System;
using System.Net;
using System.Net.Sockets;

namespace Slight.Core.Connect
{
    /// <summary>
    /// user token
    /// </summary>
    public class AsyncUserToken : IDisposable
    {
        /// <summary>
        /// current connect socket
        /// </summary>
        public Socket Socket { get; set; }

        /// <summary>
        /// socket connect time
        /// </summary>
        public DateTime ConnectTime { get; set; }

        /// <summary>
        /// remote end point 
        /// </summary>
        public EndPoint IpEndPoint { get; set; }

        /// <summary>
        ///  buffer pool
        /// </summary>
        public ByteAnnularPool BufferPool { get; set; }

        /// <summary>
        /// current packet length
        /// </summary>
        public int PacketLength { get; set; }

        /// <summary>
        /// current packet data offset
        /// </summary>
        public int PacketOffset { get; set; }

        /// <summary>
        /// current packet data buffer
        /// </summary>
        public byte[] PacketBuffer { get; set; }


        /// <summary>
        /// current packet int length buffer
        /// </summary>
        public byte[] PacketLengthBuffer = new byte[sizeof(int)];

        /// <summary>
        /// dispose this object
        /// </summary>
        public void Dispose()
        {
            BufferPool = null;
            IpEndPoint = null;
            PacketBuffer = null;
            PacketLength = 0;
            PacketOffset = 0;
            Socket?.Close();
            Socket?.Dispose();
            Socket = null;

        }


        /// <summary>
        /// buffer pool byte
        /// </summary>
        public class ByteAnnularPool : AnnularPool<byte>
        {
            /// <summary>
            /// create new byte buffer pool
            /// </summary>
            /// <param name="capacity"></param>
            public ByteAnnularPool(int capacity) : base(capacity)
            {

            }

            /// <summary>
            /// data copy
            /// </summary>
            /// <param name="sourceArray"></param>
            /// <param name="sourceIndex"></param>
            /// <param name="destinationArray"></param>
            /// <param name="destinationIndex"></param>
            /// <param name="length"></param>
            protected override void BufferCopy(ref byte[] sourceArray, int sourceIndex, ref byte[] destinationArray, int destinationIndex, int length)
            {
                Buffer.BlockCopy(sourceArray, sourceIndex, destinationArray, destinationIndex, length);
            }
        }
    }
}
