using System;
using System.Net;
using UniSpyServer.Servers.PresenceConnectionManager.Entity.Structure;
using UniSpyServer.UniSpyLib.Abstraction.Interface;

namespace UniSpyServer.Servers.PresenceConnectionManager.Application
{
    class TcpServer : UniSpyServer.UniSpyLib.Abstraction.BaseClass.Network.Tcp.Server.TcpServer
    {
        public TcpServer(Guid serverID, string serverName, IPEndPoint endpoint) : base(serverID, serverName, endpoint)
        {
        }

        protected override IClient CreateClient(IConnection connection) => new Client(connection);
    }
}