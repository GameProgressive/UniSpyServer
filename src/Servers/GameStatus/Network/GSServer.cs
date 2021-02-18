using NetCoreServer;
using System;
using System.Net;
using UniSpyLib.Network;

namespace GameStatus.Network
{
    public class GSServer : UniSpyTCPServerBase
    {
        public GSServer(Guid serverID, IPEndPoint endpoint) : base(serverID, endpoint)
        {
        }

        protected override TcpSession CreateSession()
        {
            return new GSSession(this);
        }

    }
}
