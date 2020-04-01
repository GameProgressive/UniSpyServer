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
        /// Creates a new instance of <see cref="GPCMClient"/>
        /// </summary>
        public GPCMServer(IPAddress address, int port) : base( address, port)
        {
        }

        protected override TcpSession CreateSession()
        {
            return new GPCMSession(this);
        }

    }
}