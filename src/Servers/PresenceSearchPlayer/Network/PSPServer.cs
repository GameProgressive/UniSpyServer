using NetCoreServer;
using System;
using System.Net;
using UniSpyLib.Network;

namespace PresenceSearchPlayer.Network
{
    public class PSPServer : UniSpyTCPServerBase
    {
        public PSPServer(Guid serverID, IPEndPoint endpoint) : base(serverID, endpoint)
        {
            SessionManager = new PSPSessionManager();
        }

        protected override TcpSession CreateSession() => new PSPSession(this);

    }
}
