using System;
using System.Net;
using UniSpyServer.Servers.CDKey.Entity.Structure;
using UniSpyServer.UniSpyLib.Abstraction.Interface;

namespace UniSpyServer.Servers.CDKey.Application
{
    class UdpServer : UniSpyLib.Application.Network.Udp.Server.UdpServer
    {
        public UdpServer(Guid serverID, string serverName, IPEndPoint endpoint) : base(serverID, serverName, endpoint)
        {
        }

        protected override IClient CreateClient(ISession session) => new Client(session);
    }
}