using GameSpyLib.Database.Entity;
using GameSpyLib.Network;
using NetCoreServer;
using System;
using System.Collections.Concurrent;
using System.Net;

//GPCM represents GameSpy Connection Manager
namespace PresenceConnectionManager
{
    /// <summary>
    /// This server emulates the Gamespy Client Manager Server on port 29900.
    /// This class is responsible for managing the login process.
    /// </summary>
    public class GPCMServer : TemplateTcpServer
    {
        /// <summary>
        /// Indicates the timeout of when a connecting client will be disconnected
        /// </summary>
        public const int ExpireTime = 20000;

        public static DatabaseEngine DB;

        /// <summary>
        /// List of sucessfully logged in clients (Pid => Client Obj)
        /// </summary>
        public static ConcurrentDictionary<Guid, GPCMSession> LoggedInSession = new ConcurrentDictionary<Guid, GPCMSession>();

        /// <summary>
        /// A timer that is used to Poll all connections, and removes dropped connections
        /// </summary>
        public static System.Timers.Timer PollTimer { get; protected set; }

        /// <summary>
        /// A timer that is used to batch all PlayerStatusUpdates into the database
        /// </summary>
        public static System.Timers.Timer StatusTimer { get; protected set; }

        /// <summary>
        /// Indicates whether we are closing the server down
        /// </summary>
        public bool Exiting { get; private set; } = false;

        /// <summary>
        /// Creates a new instance of <see cref="GPCMClient"/>
        /// </summary>
        public GPCMServer(string serverName, DatabaseEngine engine, IPAddress address, int port) : base(serverName, address, port)
        {
            DB = engine;
        }
        protected override TcpSession CreateSession() { return new GPCMSession(this); }


        private bool _disposed;



        protected override void Dispose(bool disposingManagedResources)
        {
            if (_disposed) return;
            _disposed = true;
            if (disposingManagedResources)
            {

            }
            PollTimer?.Stop();
            PollTimer?.Dispose();
            StatusTimer?.Stop();
            StatusTimer?.Dispose();

            base.Dispose(disposingManagedResources);
        }
    }
}
