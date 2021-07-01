using NetCoreServer;
using System;
using System.Net;
using UniSpyLib.Abstraction.BaseClass.Network.Tcp.Server;
namespace PresenceSearchPlayer.Network
{
    public sealed class PSPServer : UniSpyTcpServer
    {
        public PSPServer(Guid serverID, IPEndPoint endpoint) : base(serverID, endpoint)
        {
            SessionManager = new PSPSessionManager();
        }

        protected override TcpSession CreateSession() => new PSPSession(this);

    }
}
