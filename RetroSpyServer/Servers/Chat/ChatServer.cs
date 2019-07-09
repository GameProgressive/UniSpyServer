using System;
using System.Collections.Concurrent;
using System.Net;
using System.Threading;
using GameSpyLib.Database;
using GameSpyLib.Logging;
using GameSpyLib.Network;

namespace RetroSpyServer.Servers.PeerChat
{
        public class ChatServer : TcpServer
        {

            /// <summary>
            /// A connection counter, used to create unique connection id's
            /// </summary>
            private static long ConnectionCounter = 0;

            /// <summary>
            /// Indicates whether we are closing the server down
            /// </summary>
            public bool Exiting { get; private set; } = false;

            /// <summary>
            /// A List of sucessfully active connections (Name => Client Obj) on the MasterServer TCP line
            /// </summary>
            private static ConcurrentDictionary<long, ChatClient> Clients = new ConcurrentDictionary<long, ChatClient>();


            /// <summary>
            /// Default constructor
            /// </summary>
            /// <param name="databaseDriver">
            /// A connection to a database
            /// If the databaseDriver is null, then the server will attempt to create it's own connection
            /// otherwise it will use the specified connection
            /// </param>
            public ChatServer(string serverName, DatabaseDriver databaseDriver, IPEndPoint bindTo, int MaxConnections) : base(serverName, bindTo, MaxConnections)
            {
                ChatHelper.DBQuery = new DBQueries.ChatDBQuery(databaseDriver);

                ChatClient.OnDisconnect += PeerChatClient_OnDisconnect;

                // Begin accepting connections
                StartAcceptAsync();
            }

            ~ChatServer()
            {
                if (!Exiting)
                    Shutdown();
            }

            public void Shutdown()
            {
                // Stop accepting new connections
                IgnoreNewConnections = true;
                Exiting = true;

                // Unregister events so we dont get a shit ton of calls
                ChatClient.OnDisconnect -= PeerChatClient_OnDisconnect;

                // Disconnected all connected clients
                foreach (ChatClient c in Clients.Values)
                    c.Dispose(true);

                // clear clients
                Clients.Clear();

                // Shutdown the listener socket
                ShutdownSocket();

                ChatHelper.DBQuery.Dispose();

                // Tell the base to dispose all free objects
                Dispose();
            }

            /// <summary>
            /// This function is fired when an exception occour in the server
            /// </summary>
            /// <param name="e">The exception to be thrown</param>
            protected override void OnException(Exception e) => LogWriter.Log.WriteException(e);

            /// <summary>
            /// When a new connection is established, we the parent class are responsible
            /// for handling the processing
            /// </summary>
            /// <param name="Stream">A GamespyTcpStream object that wraps the I/O AsyncEventArgs and socket</param>
            protected override void ProcessAccept(TcpStream stream)
            {
                // Get our connection id
                long ConID = Interlocked.Increment(ref ConnectionCounter);
               ChatClient client;
                try
                {
                    // Convert the TcpClient to a MasterClient
                    client = new ChatClient(stream, ConID);
                    Clients.TryAdd(ConID, client);

                    // Start receiving data
                    stream.BeginReceive();
                }
                catch
                {
                    // Remove pending connection
                    Clients.TryRemove(ConID, out client);
                }
            }

            /// <summary>
            /// Callback for when a connection had disconnected
            /// </summary>
            /// <param name="sender">The client object whom is disconnecting</param>
            private void PeerChatClient_OnDisconnect(ChatClient client)
            {
                // Release this stream's AsyncEventArgs to the object pool
                Release(client.Stream);
                if (Clients.TryRemove(client.ConnectionID, out client) && !client.Disposed)
                    client.Dispose();
            }
        }
    }

