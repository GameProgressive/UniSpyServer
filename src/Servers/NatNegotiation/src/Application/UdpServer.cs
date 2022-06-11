using System;
using System.Net;
using UniSpyServer.Servers.NatNegotiation.Entity.Structure;
using UniSpyServer.UniSpyLib.Abstraction.Interface;

namespace UniSpyServer.Servers.NatNegotiation.Application
{
    class UdpServer : UniSpyLib.Application.Network.Udp.Server.UdpServer
    {
        public UdpServer(Guid serverID, string serverName, IPEndPoint endpoint) : base(serverID, serverName, endpoint)
        {
        }

        protected override IClient CreateClient(ISession session) => new Client(session);
    }
}