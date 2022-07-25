using System;
using System.Net;
using UniSpyServer.Servers.QueryReport.Entity.Structure;
using UniSpyServer.UniSpyLib.Abstraction.Interface;

namespace UniSpyServer.Servers.QueryReport.Application
{
    class UdpServer : UniSpyServer.UniSpyLib.Abstraction.BaseClass.Network.Udp.Server.UdpServer
    {
        public UdpServer(Guid serverID, string serverName, IPEndPoint endpoint) : base(serverID, serverName, endpoint)
        {
        }

        protected override IClient CreateClient(ISession session) => new Client(session);
    }
}