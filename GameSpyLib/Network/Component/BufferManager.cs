using System;
using System.Collections.Concurrent;
using System.Net.Sockets;

namespace GameSpyLib.Network
{
    /// <summary>
    /// This class creates a single large buffer which can be divided up 
    /// and assigned to SocketAsyncEventArgs objects for use with each 
    /// socket I/O operation. This enables buffers to be easily reused and 
    /// guards against fragmenting heap memory.
    /// </summary>
    public class BufferManager : IDisposable
    {
        /// <summary>
        /// Our buffer object
        /// </summary>
        protected byte[] Buffer;

        /// <summary>
        /// The total amount of bytes allocated by this buffer object
        /// </summary>
        public int BufferSize
        {
            get { return Buffer.Length; }
        }

        /// <summary>
        /// The number of bytes each SocketAsyncEventArgs object gets allocated
        /// inside the Buffer for all IO operations
        /// </summary>
        public readonly int BytesToAllocPerEventArg;

        /// <summary>
        /// Contains free buffer space, which can be assigned to new SAEA
        /// </summary>
        protected ConcurrentStack<BufferDataToken> FreeBufferSpace = new ConcurrentStack<BufferDataToken>();

        /// <summary>
        /// Indicates whether there is space still remaining in the buffer
        /// </summary>
        public bool SpaceAvailable
        {
            get
            {
                return !Disposed && FreeBufferSpace.Count > 0;
            }
        }

        /// <summary>
        /// Indicates whether this object has been disposed
        /// </summary>
        public bool Disposed { get; protected set; }

        /// <summary>
        /// Creates a new instance of BufferManager
        /// </summary>
        /// <param name="NumEventArgs">Specifies the maximum number of SocketAsyncEventArgs objects that will be assigned buffer space at once</param>
        /// <param name="BytesToAllocPerEventArg">Specifies the number of bytes each SocketAsyncEventArgs object will be allocated from the buffer</param>
        public BufferManager(int NumEventArgs, int BytesToAllocPerEventArg)
        {
            // Argument Checks
            if (NumEventArgs <= 0)
                throw new ArgumentException("Argument must be greater than 0", "NumEventArgs");
            else if (BytesToAllocPerEventArg <= 0)
                throw new ArgumentException("Argument must be greater than 0", "BytesToAllocPerEventArg");

            // Create the buffer array
            this.Buffer = new byte[NumEventArgs * BytesToAllocPerEventArg];
            this.BytesToAllocPerEventArg = BytesToAllocPerEventArg;
            this.Disposed = false;
            int CurrentOffset = 0;

            // Create our BufferDataTokens
            for (int i = 0; i < NumEventArgs; i++)
            {
                // Create the new data token
                FreeBufferSpace.Push(new BufferDataToken(CurrentOffset, BytesToAllocPerEventArg));

                // Increase the current offset for the next object
                CurrentOffset += BytesToAllocPerEventArg;
            }
        }

        /// <summary>
        /// Assigns a buffer space from the buffer block to the 
        /// specified SocketAsyncEventArgs object.
        /// </summary>
        /// <param name="args">The SocketEventArgs object to assign a buffer space to</param>
        /// <returns></returns>
        public bool AssignBuffer(SocketAsyncEventArgs args)
        {
            // Check for dispose
            CheckDisposed();

            // Make sure we have enough room in the buffer!
            if (!SpaceAvailable) return false;

            // Set the user token property to our BufferDataToken
            BufferDataToken Token;
            if (FreeBufferSpace.TryPop(out Token))
            {
                args.SetBuffer(Buffer, Token.BufferOffset, Token.BufferBlockSize);
                args.UserToken = Token;
                return true;
            }

            return false;
        }

        /// <summary>
        /// Releases Buffer space assigned to a token so that it can be assingned to a new SAEA
        /// </summary>
        /// <param name="args">The SocketEventArgs object that we are releasing buffer space from</param>
        public void ReleaseBuffer(SocketAsyncEventArgs args)
        {
            // Check for dispose
            CheckDisposed();

            // Grab the SAEA user token, which should be a BufferDataToken
            BufferDataToken Token = args.UserToken as BufferDataToken;
            if (Token == null)
                throw new Exception("The SocketAsyncEventArgs.UserToken was not a valid instance of BufferDataToken");

            // Add the free buffer space back
            FreeBufferSpace.Push(Token);

            // Try and reset buffer
            try { args.SetBuffer(null, 0, 0); }
            catch (ObjectDisposedException) { }
        }

        /// <summary>
        /// If Dispose() has been called on this object, an ObjectDisposedException will be thrown here
        /// </summary>
        private void CheckDisposed()
        {
            if (Disposed)
                throw new ObjectDisposedException("BufferManager");
        }

        /// <summary>
        /// Releases all bytes held by this buffer
        /// </summary>
        public void Dispose()
        {
            this.Buffer = null;
            this.Disposed = true;
        }
    }
}
