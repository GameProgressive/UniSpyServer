using System;
using System.Net;
using System.Net.Sockets;
using System.Threading;
using GameSpyLib.Database;

namespace GameSpyLib.Network
{
    /// <summary>
    /// Like the TCPServer, this class represents a high perfomance
    /// implementable UDP server
    /// </summary>
    public abstract class UDPServer : TemplateServer
    {
        /// <summary>
        /// The amount of bytes each SocketAysncEventArgs object
        /// will get assigned to in the buffer manager.
        /// </summary>
        protected readonly int BufferSizePerEvent = 8192;

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
        /// A pool of reusable SocketAsyncEventArgs objects for receive and send socket operations
        /// </summary>
        protected SocketAsyncEventArgsPool SocketReadWritePool;

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="databaseDriver">
        /// A connection to a database that could be used in the server.
        /// Set this parameter to null if the server does not require a database connection.
        /// </param>
        public UDPServer(DatabaseDriver databaseDriver) : base(databaseDriver)
        {
        }

        /// <summary>
        /// Starts an UDP server
        /// </summary>
        /// <param name="endPoint">The IP Endpoint to bind the server</param>
        /// <param name="maxConnections">Max connections allowed</param>
        public override void Start(IPEndPoint bindTo, int MaxConnections)
        {
            // Create our Socket
            Listener = new Socket(AddressFamily.InterNetwork, SocketType.Dgram, ProtocolType.Udp)
            {
                SendTimeout = 5000, // We have a limited pool, so we dont want to be locked often
                SendBufferSize = BufferSizePerEvent,
                ReceiveBufferSize = BufferSizePerEvent
            };

            // Bind to our port
            Listener.Bind(bindTo);

            // Set the rest of our internals
            MaxNumConnections = MaxConnections;
            MaxConnectionsEnforcer = new SemaphoreSlim(MaxNumConnections, MaxNumConnections);
            SocketReadWritePool = new SocketAsyncEventArgsPool(MaxNumConnections);

            // Create our Buffer Manager for IO operations. 
            BufferManager = new BufferManager(
                MaxNumConnections,
                BufferSizePerEvent
            );

            // Assign our Connection IO SocketAsyncEventArgs object instances
            for (int i = 0; i < MaxNumConnections; i++)
            {
                SocketAsyncEventArgs SockArg = new SocketAsyncEventArgs();
                SockArg.Completed += IOComplete;
                BufferManager.AssignBuffer(SockArg);
                SocketReadWritePool.Push(SockArg);
            }

            // set public internals
            Port = bindTo.Port;
            Address = bindTo.Address.ToString();
            IsRunning = true;
            IsDisposed = false;

            // Start accepting sockets
            StartAcceptAsync();
        }

        /// <summary>
        /// Releases all Objects held by this socket. Will also
        /// shutdown the socket if its still running
        /// </summary>
        public override void Dispose()
        {
            // no need to do this again
            if (IsDisposed) return;
            IsDisposed = true;

            // Shutdown sockets
            Stop();

            // Dispose all ReadWritePool AysncEventArg objects
            while (SocketReadWritePool.Count > 0)
                SocketReadWritePool.Pop().Dispose();

            // Dispose the buffer manager after disposing all EventArgs
            BufferManager.Dispose();
            MaxConnectionsEnforcer.Dispose();
            Listener.Dispose();
            databaseDriver?.Dispose();
        }

        /// <summary>
        /// When called, this method will stop accepted and handling any and all
        /// connections. Dispose still needs to be called!
        /// </summary>
        public override void Stop()
        {
            // Only do this once
            if (!IsRunning) return;
            IsRunning = false;

            // Stop accepting connections
            if (Listener.Connected)
                Listener.Shutdown(SocketShutdown.Both);

            // Close the listener
            Listener.Close();
        }

        /// <summary>
        /// Releases the SocketAsyncEventArgs back to the pool,
        /// and free's up another slot for a new client to connect
        /// </summary>
        /// <param name="e"></param>
        protected void Release(SocketAsyncEventArgs e)
        {
            // Get our ReadWrite AsyncEvent object back
            SocketReadWritePool.Push(e);

            // Now that we have another set of AsyncEventArgs, we can
            // release this users Semephore lock, allowing another connection
            MaxConnectionsEnforcer.Release();
        }

        /// <summary>
        /// Begins accepting a new Connection asynchronously
        /// </summary>
        protected async void StartAcceptAsync()
        {
            // If we are shutting down, dont receive again
            if (!IsRunning) return;

            try
            {
                // Enforce max connections. If we are capped on connections, the new connection will stop here,
                // and retrun once a connection is opened up from the Release() method
                await MaxConnectionsEnforcer.WaitAsync();

                // Fetch ourselves an available AcceptEventArg for the next connection
                SocketAsyncEventArgs AcceptEventArg = SocketReadWritePool.Pop();
                AcceptEventArg.RemoteEndPoint = new IPEndPoint(IPAddress.Any, Port);

                // Reset the Async's Buffer position for the next read
                BufferDataToken token = AcceptEventArg.UserToken as BufferDataToken;
                AcceptEventArg.SetBuffer(token.BufferOffset, token.BufferBlockSize);

                // Begin accpetion connections
                bool willRaiseEvent = Listener.ReceiveFromAsync(AcceptEventArg);

                // If we wont raise event, that means a connection has already been accepted syncronously
                // and the Accept_Completed event will NOT be fired. So we manually call ProcessAccept
                if (!willRaiseEvent)
                    IOComplete(this, AcceptEventArg);
            }
            catch (ObjectDisposedException)
            {
                // Happens when the server is shutdown
            }
            catch (Exception e)
            {
                OnException(e);
            }
        }

        /// <summary>
        /// Sends the specified packets data to the client, and releases the resources
        /// </summary>
        /// <param name="Packet"></param>
        protected void ReplyAsync(UDPPacket Packet)
        {
            // If we are shutting down, dont receive again
            if (!IsRunning) return;

            Listener.SendToAsync(Packet.AsyncEventArgs);
        }

        /// <summary>
        /// Once a connection has been received, its handed off here to convert it into
        /// our client object, and prepared to be handed off to the parent for processing
        /// </summary>
        /// <param name="AcceptEventArg"></param>
        private void IOComplete(object sender, SocketAsyncEventArgs AcceptEventArg)
        {
            if (AcceptEventArg.LastOperation == SocketAsyncOperation.ReceiveFrom)
            {
                // If we do not get a success code here, we have a bad socket
                if (AcceptEventArg.SocketError != SocketError.Success)
                {
                    // Put the SAEA back in the pool.
                    SocketReadWritePool.Push(AcceptEventArg);
                    StartAcceptAsync();
                    return;
                }

                // Begin accepting a new connection
                StartAcceptAsync();

                UDPPacket packet = new UDPPacket(AcceptEventArg);

                if (Logger.DebugSocket)
                    Logger.Debug("UDP Operation " + AcceptEventArg.LastOperation.ToString() + " : " + BitConverter.ToString(packet.BytesRecieved).Replace("-", ""));

                // Hand off processing to the parent
                ProcessAccept(packet);
            }
            else
            {
                Release(AcceptEventArg);
            }
        }

        /// <summary>
        /// When a new connection is established, the parent class is responsible for
        /// processing the connected client
        /// </summary>
        /// <param name="Packet">A Udp Packet object that wraps the I/O AsyncEventArgs and socket</param>
        protected abstract void ProcessAccept(UDPPacket Packet);

        /// <summary>
        /// This function is fired when an exception oncurrs
        /// </summary>
        /// <param name="e">The exception parameter</param>
        protected abstract void OnException(Exception e);
    }
}
