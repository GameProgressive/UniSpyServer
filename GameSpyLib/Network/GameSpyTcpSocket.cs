using System;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using GameSpyLib.Logging;

namespace GameSpyLib.Network
{
    /// <summary>
    /// This class represents a high performance Async Tcp Socket wrapper
    /// that is used to act as a base for all Gamespy protocol Tcp
    /// connections.
    /// </summary>
    public abstract class GamespyTcpSocket : IDisposable
    {
        /// <summary>
        /// Max number of concurrent open and active connections. Increasing this number
        /// will also increase the IO Buffer by <paramref name="BufferSizePerOperation"/> * 2
        /// </summary>
        protected readonly int MaxNumConnections;

        /// <summary>
        /// The initial size of the conccurent accept pool
        /// when accepting new connections. High volume of connections
        /// will increase the pool size if need be
        /// <remarks>4 should be a pretty good init compacity since accepting a client is pretty fast</remarks>
        /// </summary>
        protected readonly int ConcurrentAcceptPoolSize = 4;

        /// <summary>
        /// The amount of bytes each SocketAysncEventArgs object
        /// will get assigned to in the buffer manager.
        /// </summary>
        protected readonly int BufferSizePerOperation = 256;

        /// <summary>
        /// Our Listening Socket
        /// </summary>
        protected Socket Listener;

        /// <summary>
        /// Buffers for sockets are unmanaged by .NET, which means that
        /// memory will get fragmented because the GC can't touch these
        /// byte arrays. So we will manage our buffers manually
        /// </summary>
        protected BufferManager BufferManager;

        /// <summary>
        /// Use a Semaphore to prevent more then the MaxNumConnections
        /// clients from logging in at once.
        /// </summary>
        protected SemaphoreSlim MaxConnectionsEnforcer;

        /// <summary>
        /// Determines where the MaxConnectionsEnforcer stops to wait for a spot to open, and allow
        /// the connecting client through.
        /// </summary>
        protected EnforceMode ConnectionEnforceMode = EnforceMode.BeforeAccept;

        /// <summary>
        /// If the ConnectionEnforceMode is set to DuringPrepare, then the wait timeout is set here
        /// </summary>
        protected int WaitTimeout = 500;

        /// <summary>
        /// The error message to display if the server is full. Only works if ConnectionEnforceMode is set to DuringPrepare.
        /// If left empty, no message will be sent back to the client
        /// </summary>
        protected string FullErrorMessage = string.Empty;

        /// <summary>
        /// A pool of reusable SocketAsyncEventArgs objects for accept operations
        /// </summary>
        protected SocketAsyncEventArgsPool SocketAcceptPool;

        /// <summary>
        /// A pool of reusable SocketAsyncEventArgs objects for receive and send socket operations
        /// </summary>
        protected SocketAsyncEventArgsPool SocketReadWritePool;

        /// <summary>
        /// Indicates whether the socket is listening for connections
        /// </summary>
        public bool IsListening { get; protected set; }

        /// <summary>
        /// If set to True, new connections will be immediatly closed and ignored.
        /// </summary>
        public bool IgnoreNewConnections { get; protected set; } = false;

        /// <summary>
        /// Indicates whether this object has been disposed yet
        /// </summary>
        public bool IsDisposed { get; protected set; } = false;

        public GamespyTcpSocket(IPEndPoint bindTo, int MaxConnections)
        {
            // Create our Socket
            Listener = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);

            // Set Socket options
            Listener.SetSocketOption(SocketOptionLevel.Socket, SocketOptionName.DontLinger, true);
            Listener.SetSocketOption(SocketOptionLevel.Socket, SocketOptionName.Linger, false);
            Listener.SetSocketOption(SocketOptionLevel.Socket, SocketOptionName.KeepAlive, true);

            // Bind to our port
            Listener.Bind(bindTo);
            Listener.Listen(25);

            // Set the rest of our internals
            MaxNumConnections = MaxConnections;
            MaxConnectionsEnforcer = new SemaphoreSlim(MaxNumConnections, MaxNumConnections);
            SocketAcceptPool = new SocketAsyncEventArgsPool(ConcurrentAcceptPoolSize);
            SocketReadWritePool = new SocketAsyncEventArgsPool(MaxNumConnections * 2);

            // Create our Buffer Manager for IO operations. 
            // Always allocate double space, one for recieving, and another for sending
            BufferManager = new BufferManager(MaxNumConnections * 2, BufferSizePerOperation);

            // Assign our Connection Accept SocketAsyncEventArgs object instances
            for (int i = 0; i < ConcurrentAcceptPoolSize; i++)
            {
                SocketAsyncEventArgs SockArg = new SocketAsyncEventArgs();
                SockArg.Completed += (s, e) => PrepareAccept(e);

                // Do NOT assign buffer space for Accept operations!               
                SocketAcceptPool.Push(SockArg);
            }

            // Assign our Connection IO SocketAsyncEventArgs object instances
            for (int i = 0; i < MaxNumConnections * 2; i++)
            {
                SocketAsyncEventArgs SockArg = new SocketAsyncEventArgs();
                BufferManager.AssignBuffer(SockArg);
                SocketReadWritePool.Push(SockArg);
            }

            // set public internals
            IsListening = true;
        }

        ~GamespyTcpSocket()
        {
            if (!IsDisposed)
                Dispose();
        }

        /// <summary>
        /// Releases all Objects held by this socket. Will also
        /// shutdown the socket if its still running
        /// </summary>
        public void Dispose()
        {
            // no need to do this again
            if (IsDisposed) return;
            IsDisposed = true;

            // Shutdown sockets
            if (IsListening)
                ShutdownSocket();

            // Dispose all AcceptPool AysncEventArg objects
            while (SocketAcceptPool.Count > 0)
                SocketAcceptPool.Pop().Dispose();

            // Dispose all ReadWritePool AysncEventArg objects
            while (SocketReadWritePool.Count > 0)
                SocketReadWritePool.Pop().Dispose();

            // Dispose the buffer manager after disposing all EventArgs
            BufferManager?.Dispose();
            MaxConnectionsEnforcer?.Dispose();
            Listener?.Dispose();
        }

        /// <summary>
        /// When called, this method will stop accepted and handling any and all
        /// connections. Dispose still needs to be called!
        /// </summary>
        protected void ShutdownSocket()
        {
            // Only do this once
            if (!IsListening) return;
            IsListening = false;

            // Stop accepting connections
            try
            {
                Listener.Shutdown(SocketShutdown.Both);
            }
            catch { }

            // Close the listener
            Listener.Close();
        }

        /// <summary>
        /// Releases the Stream's SocketAsyncEventArgs back to the pool,
        /// and free's up another slot for a new client to connect
        /// </summary>
        /// <param name="Stream">The GamespyTcpStream object that is being released.</param>
        public void Release(GamespyTcpStream Stream)
        {
            // If the stream has been released, then we stop here
            if (!IsListening || Stream.Released) return;

            // Make sure the connection is closed properly
            if (!Stream.SocketClosed)
            {
                Stream.Close();
                return;
            }

            // To prevent cross instance releasing
            //if (!Object.ReferenceEquals(this, Stream.SocketManager))
            //throw new ArgumentException("Cannot pass a GamespyTcpStream belonging to a different TcpSocket than this one.");

            // If we are still registered for this event, then the EventArgs should
            // NEVER be disposed here, or we have an error to fix
            if (Stream.DisposedEventArgs)
            {
                // Dispose old buffer tokens
                BufferManager.ReleaseBuffer(Stream.ReadEventArgs);
                BufferManager.ReleaseBuffer(Stream.WriteEventArgs);

                // Create new Read Event Args
                SocketAsyncEventArgs SockArgR = new SocketAsyncEventArgs();
                BufferManager.AssignBuffer(SockArgR);
                SocketReadWritePool.Push(SockArgR);

                // Create new Write Event Args
                SocketAsyncEventArgs SockArgW = new SocketAsyncEventArgs();
                BufferManager.AssignBuffer(SockArgW);
                SocketReadWritePool.Push(SockArgW);
            }
            else
            {
                // Set null's
                Stream.ReadEventArgs.AcceptSocket = null;
                Stream.WriteEventArgs.AcceptSocket = null;

                // Get our ReadWrite AsyncEvent object back
                SocketReadWritePool.Push(Stream.ReadEventArgs);
                SocketReadWritePool.Push(Stream.WriteEventArgs);
            }

            // Now that we have another set of AsyncEventArgs, we can
            // release this users Semephore lock, allowing another connection
            MaxConnectionsEnforcer.Release();
        }

        /// <summary>
        /// Begins accepting a new Connection asynchronously
        /// </summary>
        protected async void StartAcceptAsync()
        {
            // Fetch ourselves an available AcceptEventArg for the next connection
            SocketAsyncEventArgs AcceptEventArg;
            if (SocketAcceptPool.Count > 0)
            {
                try
                {
                    AcceptEventArg = SocketAcceptPool.Pop();
                }
                catch
                {
                    AcceptEventArg = new SocketAsyncEventArgs();
                    AcceptEventArg.Completed += (s, e) => PrepareAccept(e);
                }
            }
            else
            {
                // NO SOCKS AVAIL!
                AcceptEventArg = new SocketAsyncEventArgs();
                AcceptEventArg.Completed += (s, e) => PrepareAccept(e);
            }

            try
            {
                // Enforce max connections. If we are capped on connections, the new connection will stop here,
                // and retrun once a connection is opened up from the Release() method
                if (ConnectionEnforceMode == EnforceMode.BeforeAccept)
                    await MaxConnectionsEnforcer.WaitAsync();

                // Begin accpetion connections
                bool willRaiseEvent = Listener.AcceptAsync(AcceptEventArg);

                // If we wont raise event, that means a connection has already been accepted syncronously
                // and the Accept_Completed event will NOT be fired. So we manually call ProcessAccept
                if (!willRaiseEvent)
                    PrepareAccept(AcceptEventArg);
            }
            catch (ObjectDisposedException)
            {
                // Happens when the server is shutdown
            }
        }

        /// <summary>
        /// Once a connection has been received, its handed off here to convert it into
        /// our client object, and prepared to be handed off to the parent for processing
        /// </summary>
        /// <param name="AcceptEventArg"></param>
        protected async void PrepareAccept(SocketAsyncEventArgs AcceptEventArg)
        {
            // If we do not get a success code here, we have a bad socket
            if (IgnoreNewConnections || AcceptEventArg.SocketError != SocketError.Success)
            {
                // This method closes the socket and releases all resources, both
                // managed and unmanaged. It internally calls Dispose.           
                AcceptEventArg.AcceptSocket.Close();

                // Put the SAEA back in the pool.
                SocketAcceptPool.Push(AcceptEventArg);
                StartAcceptAsync();
                return;
            }

            // If the server is full, send an error message to the player
            if (ConnectionEnforceMode == EnforceMode.DuringPrepare)
            {
                bool Success = await MaxConnectionsEnforcer.WaitAsync(WaitTimeout);
                if (!Success)
                {
                    // If we arent even listening...
                    if (!IsListening) return;

                    // Alert the client that we are full
                    if (!string.IsNullOrEmpty(FullErrorMessage))
                    {
                        byte[] buffer = Encoding.UTF8.GetBytes(
                            string.Format(@"\error\\err\0\fatal\\errmsg\{0}\id\1\final\", FullErrorMessage)
                        );
                        AcceptEventArg.AcceptSocket.Send(buffer);
                    }

                    // Log so we can track this!
                    LogWriter.Log.Write("The Server is currently full! Rejecting connecting client.", LogLevel.Info);

                    // Put the SAEA back in the pool.
                    AcceptEventArg.AcceptSocket.Close();
                    SocketAcceptPool.Push(AcceptEventArg);
                    StartAcceptAsync();
                    return;
                }
            }

            // Begin accepting a new connection
            StartAcceptAsync();

            // Grab a send/recieve object
            SocketAsyncEventArgs ReadArgs = SocketReadWritePool.Pop();
            SocketAsyncEventArgs WriteArgs = SocketReadWritePool.Pop();

            // Pass over the reference to the new socket that is handling
            // this specific stream, and dereference it so we can hand the
            // acception event back over
            ReadArgs.AcceptSocket = WriteArgs.AcceptSocket = AcceptEventArg.AcceptSocket;
            AcceptEventArg.AcceptSocket = null;

            // Hand back the AcceptEventArg so another connection can be accepted
            SocketAcceptPool.Push(AcceptEventArg);

            // Hand off processing to the parent
            GamespyTcpStream Stream = null;
            try
            {
                Stream = new GamespyTcpStream(this, ReadArgs, WriteArgs);
                ProcessAccept(Stream);
            }
            catch (Exception e)
            {
                // Report Error
                OnException(e);

                // Make sure the connection is closed properly
                if (Stream != null)
                    Release(Stream);
            }
        }

        /// <summary>
        /// When a new connection is established, the parent class is responsible for
        /// processing the connected client
        /// </summary>
        /// <param name="Stream">A GamespyTcpStream object that wraps the I/O AsyncEventArgs and socket</param>
        protected abstract void ProcessAccept(GamespyTcpStream Stream);

        protected abstract void OnException(Exception e);
    }

    public enum EnforceMode
    {
        BeforeAccept, DuringPrepare
    }
}
