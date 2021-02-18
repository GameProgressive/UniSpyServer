using NetCoreServer;
using System;
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
        public PCMServer(Guid serverID, IPEndPoint endpoint) : base(serverID, endpoint)
        {
        }

        protected override TcpSession CreateSession()
        {
            return new PCMSession(this);
        }

    }
}