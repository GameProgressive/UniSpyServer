using NetCoreServer;
using System;
using System.Collections.Concurrent;
using System.Net;
using UniSpyLib.Network;

//GPCM represents GameSpy Connection Manager
namespace PresenceConnectionManager.Network
{
    /// <summary>
    /// This server emulates the Gamespy Client Manager Server on port 29900.
    /// This class is responsible for managing the login process.
    /// </summary>
    internal sealed class PCMServer : UniSpyTCPServerBase
    {
        /// <summary>
        /// List of sucessfully logged in clients (Pid => Client Obj)
        /// </summary>
        public static ConcurrentDictionary<Guid, PCMSession> LoggedInSession;

        public PCMServer(IPAddress address, int port) : base(address, port)
        {
            LoggedInSession = new ConcurrentDictionary<Guid, PCMSession>();
        }

        /// <summary>
        /// Creates a new instance of <see cref="GPCMClient"/>
        /// </summary>
        protected override TcpSession CreateSession()
        {
            return new PCMSession(this);
        }

    }
}