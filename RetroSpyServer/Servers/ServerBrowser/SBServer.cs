using System;
using System.Collections.Concurrent;
using System.IO;
using System.Net;
using System.Threading;
using GameSpyLib.Network;
using GameSpyLib.Logging;
using GameSpyLib.Database;

namespace RetroSpyServer.Servers.ServerBrowser
{
    /// <summary>
    /// This class emulates the master.gamespy.com TCP server on port 28910.
    /// This server is responisible for sending server lists to the online server browser in the game.
    /// </summary>
    public class SBServer : TcpServer
    {
        /// <summary>
        /// A connection counter, used to create unique connection id's
        /// </summary>
        private long ConnectionCounter = 0;

        /// <summary>
        /// A List of sucessfully active connections (Name => Client Obj) on the MasterServer TCP line
        /// </summary>
        private static ConcurrentDictionary<long, SBClient> Clients = new ConcurrentDictionary<long, SBClient>();

        public SBServer(string serverName,IPEndPoint bindTo,int MaxConnections) : base(serverName,bindTo, MaxConnections)
        {
            // Start accepting connections
            SBClient.OnDisconnect += ClientDisconnected;
            StartAcceptAsync();
        }

        /// <summary>
        /// Shutsdown the underlying sockets
        /// </summary>
        public void Shutdown()
        {
            // Stop accepting new connections
            base.IgnoreNewConnections = true;

            // Unregister events so we dont get a shit ton of calls
            SBClient.OnDisconnect -= ClientDisconnected;

            // Disconnected all connected clients
            foreach (SBClient client in Clients.Values)
                client.Dispose(true);

            // Update Connected Clients in the Database
            Clients.Clear();

            // Shutdown the listener socket
            ShutdownSocket();

            // Tell the base to dispose all free objects
            Dispose();
        }

        /// <summary>
        /// Accepts a TcpClient, and begin the serverlist fetching process for the client. 
        /// This method is executed when the user updates his server browser ingame
        /// </summary>
        protected override void ProcessAccept(TcpStream Stream)
        {
            // Get our connection id
            long ConID = Interlocked.Increment(ref ConnectionCounter);
            SBClient client;

            // End the operation and display the received data on  
            // the console.
            try
            {
                // Convert the TcpClient to a MasterClient
                client = new SBClient(Stream, ConID);
                Clients.TryAdd(client.ConnectionID, client);

                // Start receiving data
                Stream.BeginReceive();
            }
            catch (ObjectDisposedException) // Ignore
            {
                // Remove client
                Clients.TryRemove(ConID, out client);
            }
            catch (IOException) // Connection closed before a TcpClientStream could be made
            {
                // Remove client
                Clients.TryRemove(ConID, out client);
            }
            catch (Exception e)
            {
                // Remove client
                Clients.TryRemove(ConID, out client);

                // Report error

                LogWriter.Log.Write( LogLevel.Error, "[SB] : An Error occured at : {0}", e.ToString());

                //ExceptionHandler.GenerateExceptionLog(e);
            }
        }

        /// <summary>
        /// Callback for when a connection had disconnected
        /// </summary>
        protected void ClientDisconnected(SBClient client)
        {
            // Remove client, and call OnUpdate Event
            try
            {
                // Release this stream's AsyncEventArgs to the object pool
                base.Release(client.Stream);

                // Remove client from online list
                if (Clients.TryRemove(client.ConnectionID, out client) && !client.Disposed)
                    client.Dispose();
            }
            catch (Exception e)
            {
                LogWriter.Log.Write(LogLevel.Error, "[SB]: An Error occured at : {0}", e.ToString());
            }
        }

        protected override void OnException(Exception e) => LogWriter.Log.WriteException(e);

    }
}
