using System;
using System.Net.Sockets;

namespace GameSpyLib.Network
{
    /// <summary>
    /// This class represents a packet recieved from and/or to be sent to a remote UDP connection. 
    /// This class is essentially a wrapper for SocketAsyncEventArgs and Asynchronous reading and writing
    /// </summary>
    public class UdpPacket
    {
        /// <summary>
        /// Our AsycnEventArgs object for reading/writing data
        /// </summary>
        public SocketAsyncEventArgs AsyncEventArgs { get; protected set; }

        /// <summary>
        /// An array of the bytes received in this packet
        /// </summary>
        public byte[] BytesRecieved;

        public UdpPacket(SocketAsyncEventArgs e)
        {
            // Get our recived bytes
            BytesRecieved = new byte[e.BytesTransferred];
            BufferDataToken token = e.UserToken as BufferDataToken;
            Array.Copy(e.Buffer, token.BufferOffset, BytesRecieved, 0, e.BytesTransferred);

            // Set our internal variables
            AsyncEventArgs = e;
        }

        /// <summary>
        /// Sets the contents of the SocketAsyncEventArgs buffer,
        /// so a reply can be sent to the remote host connection
        /// </summary>
        /// <param name="contents">The new contents to set the buffer to</param>
        /// <returns>The length of bytes written to the buffer</returns>
        public int SetBufferContents(byte[] contents)
        {
            BufferDataToken token = AsyncEventArgs.UserToken as BufferDataToken;
            if (contents.Length > token.BufferBlockSize)
                throw new ArgumentOutOfRangeException("contents", "Contents are larger then the allocated buffer block size.");

            // Copy contents to buffer, then set buffer position
            Array.Copy(contents, 0, AsyncEventArgs.Buffer, token.BufferOffset, contents.Length);
            AsyncEventArgs.SetBuffer(token.BufferOffset, contents.Length);
            return contents.Length;
        }        
    }
}
