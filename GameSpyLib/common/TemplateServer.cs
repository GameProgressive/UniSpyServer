using System;
using System.Net;
using System.Net.Sockets;
using GameSpyLib.Database;

namespace GameSpyLib.Network
{
    /// <summary>
    /// This class represents an implementable server
    /// </summary>
    public abstract class TemplateServer : IDisposable
    {
        /// <summary>
        /// A connection to a database
        /// </summary>
        protected DatabaseDriver databaseDriver = null;

        /// <summary>
        /// Indicates whether the server is still running, and not in the process of shutting down
        /// </summary>
        public bool IsRunning { get; protected set; }

        /// <summary>
        /// Indicates whether this object has been disposed yet
        /// </summary>
        public bool IsDisposed { get; protected set; }

        /// <summary>
        /// The port we are listening on
        /// </summary>
        public int Port { get; protected set; }

        /// <summary>
        /// The IP Address we are listening on
        /// </summary>
        public string Address { get; protected set; }

        /// <summary>
        /// Our Listening Socket
        /// </summary>
        protected Socket Listener;

        /// <summary>
        /// Max number of concurrent open and active connections.
        /// </summary>
        protected int MaxNumConnections;

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="databaseDriver">
        /// A connection to a database that could be used in the server.
        /// Set this parameter to null if the server does not require a database connection.
        /// </param>
        public TemplateServer(DatabaseDriver databaseDriver)
        {
            this.databaseDriver = databaseDriver;

            IsRunning = false;
            IsDisposed = true;
        }

        /// <summary>
        /// Default destructor
        /// </summary>
        ~TemplateServer()
        {
            if (!IsDisposed)
                Dispose();
        }

        /// <summary>
        /// Stops the server
        /// </summary>
        public abstract void Stop();

        /// <summary>
        /// Starts the server
        /// </summary>
        /// <param name="ip">The IP to bind</param>
        /// <param name="port">The port to bind</param>
        /// <param name="maxConnections">Max connections allowed</param>
        public void Start(string ip, int port, int maxConnections)
        {
            Start(new IPEndPoint(IPAddress.Parse(ip), port), maxConnections);
        }

        /// <summary>
        /// Starts the server
        /// </summary>
        /// <param name="endPoint">The IP Endpoint to bind the server</param>
        /// <param name="maxConnections">Max connections allowed</param>
        public abstract void Start(IPEndPoint endPoint, int maxConnections);

        /// <summary>
        /// Releases all objects created by the server
        /// </summary>
        public abstract void Dispose();
    }
}
