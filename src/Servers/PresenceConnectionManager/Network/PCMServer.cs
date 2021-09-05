using NetCoreServer;
using System;
using System.Net;
using UniSpyLib.Abstraction.BaseClass.Network.Tcp.Server;

//GPCM represents GameSpy Connection Manager
namespace PresenceConnectionManager.Network
{
    /// <summary>
    /// This server emulates the Gamespy Client Manager Server on port 29900.
    /// This class is responsible for managing the login process.
    /// </summary>
    internal sealed class PCMServer : UniSpyTcpServer
    {
        public PCMServer(Guid serverID, IPEndPoint endpoint) : base(serverID, endpoint)
        {
            SessionManager = new PCMSessionManager();
        }

        protected override TcpSession CreateSession() => new PCMSession(this);

    }
}