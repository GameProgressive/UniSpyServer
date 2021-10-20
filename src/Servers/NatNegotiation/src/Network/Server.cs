using UniSpyServer.NatNegotiation.Handler.CmdSwitcher;
using System;
using System.Net;
using UniSpyServer.UniSpyLib.Abstraction.BaseClass.Network.Udp.Server;

namespace UniSpyServer.NatNegotiation.Network
{
    public sealed class Server : UniSpyUdpServer
    {
        public Server(Guid serverID, IPEndPoint endpoint) : base(serverID, endpoint)
        {
            SessionManager = new SessionManager();
        }

        protected override UniSpyUdpSession CreateSession(EndPoint endPoint) =>
            new Session(this, endPoint);

        //TODO fix the natnegotiation for this architecture
    }
}
