using NetCoreServer;
using System;
using System.Net;
using UniSpyLib.Network;

namespace GameStatus.Network
{
    internal sealed class GSServer : UniSpyTCPServerBase
    {
        public GSServer(Guid serverID, IPEndPoint endpoint) : base(serverID, endpoint)
        {
            SessionManager = new GSSessionManager();
        }

        protected override TcpSession CreateSession() => new GSSession(this);
    }
}
