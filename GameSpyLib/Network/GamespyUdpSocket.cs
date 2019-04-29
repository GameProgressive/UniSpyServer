using System;
using System.Net;
using System.Net.Sockets;
using System.Threading;
using GameSpyLib.Logging;

namespace GameSpyLib.Network
{
    /// <summary>
    /// Like the GamespyTcpSocket, this class represents a high perfomance
    /// UDP socket server
    /// </summary>
    public abstract class GamespyUdpSocket : IDisposable
    {
        /// <summary>
        /// Max number of concurrent open and active connections.
        /// </summary>
        protected readonly int MaxNumConnections;

        /// <summary>
        /// The amount of bytes each SocketAysncEventArgs object
        /// will get assigned to in the buffer manager.
        /// </summary>
        protected readonly int BufferSizePerEvent = 8192;

        /// <summary>
        /// Our Listening Socket
        /// </summary>
        protected Socket Listener;

        /// <summary>
        /// The port we are listening on
        /// </summary>
        public int Port { get; protected set; }

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
        /// Indicates whether the server is still running, and not in the process of shutting down
        /// </summary>
        public bool IsRunning { get; protected set; }

        /// <summary>
        /// Indicates whether this object has been disposed yet
        /// </summary>
        public bool IsDisposed { get; protected set; }

        public GamespyUdpSocket(IPEndPoint bindTo, int MaxConnections)
        {
            // Create our Socket
            this.Port = Port;
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
            IsRunning = true;
            IsDisposed = false;
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
            if (IsRunning)
                ShutdownSocket();

            // Dispose all ReadWritePool AysncEventArg objects
            while (SocketReadWritePool.Count > 0)
                SocketReadWritePool.Pop().Dispose();

            // Dispose the buffer manager after disposing all EventArgs
            BufferManager.Dispose();
            MaxConnectionsEnforcer.Dispose();
            Listener.Dispose();
        }

        /// <summary>
        /// When called, this method will stop accepted and handling any and all
        /// connections. Dispose still needs to be called!
        /// </summary>
        protected void ShutdownSocket()
        {
            // Only do this once
            if (!IsRunning) return;
            IsRunning = false;

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
        /// Releases the SocketAsyncEventArgs back to the pool,
        /// and free's up another slot for a new client to connect
        /// </summary>
        /// <param name="Stream"></param>
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
        protected void ReplyAsync(GamespyUdpPacket Packet)
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

                GamespyUdpPacket packet = new GamespyUdpPacket(AcceptEventArg);

                if (LogWriter.Log.DebugSockets)
                    LogWriter.Log.Write("UDP operation: " + AcceptEventArg.LastOperation.ToString() + " : " + BitConverter.ToString(packet.BytesRecieved).Replace("-", ""), LogLevel.Debug);

                // Begin accepting a new connection
                StartAcceptAsync();

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
        /// <param name="Stream">A GamespyTcpStream object that wraps the I/O AsyncEventArgs and socket</param>
        protected abstract void ProcessAccept(GamespyUdpPacket Packet);

        protected abstract void OnException(Exception e);
    }
}
