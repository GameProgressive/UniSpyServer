using NetCoreServer;
using System;
using System.Net;
using UniSpyLib.Abstraction.BaseClass.Network.Tcp.Server;

namespace GameStatus.Network
{
    internal sealed class GSServer : UniSpyTcpServer
    {
        public GSServer(Guid serverID, IPEndPoint endpoint) : base(serverID, endpoint)
        {
            SessionManager = new GSSessionManager();
        }

        protected override TcpSession CreateSession() => new GSSession(this);
    }
}
