using UniSpyLib.Network;
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
    public class PCMServer : TemplateTcpServer
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