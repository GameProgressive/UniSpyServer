using GameSpyLib.Database;
using GameSpyLib.Logging;
using GameSpyLib.Network;
using System;
using System.Collections.Concurrent;
using System.Net;
using System.Threading;

namespace StatsAndTracking
{
    public class GStatsServer : TcpServer
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
        private static ConcurrentDictionary<long, GStatsClient> Clients = new ConcurrentDictionary<long, GStatsClient>();

        /// <summary>
        /// Default constructor
        /// </summary>
        /// <param name="databaseDriver">
        /// A connection to a database
        /// If the databaseDriver is null, then the server will attempt to create it's own connection
        /// otherwise it will use the specified connection
        /// </param>
        public GStatsServer(string serverName,DatabaseDriver databaseDriver, IPEndPoint bindTo, int MaxConnections) : base(serverName,bindTo, MaxConnections)
        {
            GStatsHandler.DBQuery = new GSTATSDBQuery(databaseDriver);

            // Begin accepting connections
            StartAcceptAsync();
        }

        ~GStatsServer()
        {
            if (!Exiting)
                Shutdown();
        }

        public void Shutdown()
        {
            // Stop accepting new connections
            IgnoreNewConnections = true;
            Exiting = true;

            // Disconnected all connected clients
            foreach (GStatsClient c in Clients.Values)
                c.Dispose(true);

            // clear clients
            Clients.Clear();

            // Shutdown the listener socket
            ShutdownSocket();

            GStatsHandler.DBQuery.Dispose();

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
            GStatsClient client;
            try
            {
                // Convert the TcpClient to a MasterClient
                client = new GStatsClient(stream, ConID);
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

    }
}
