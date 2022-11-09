using System;
using System.Net;
using UniSpyServer.Servers.Chat.Entity.Structure;
using UniSpyServer.UniSpyLib.Abstraction.Interface;

namespace UniSpyServer.Servers.Chat.Application
{
    class TcpServer : UniSpyLib.Abstraction.BaseClass.Network.Tcp.Server.TcpServer
    {
        public TcpServer(Guid serverID, string serverName, IPEndPoint endpoint) : base(serverID, serverName, endpoint)
        {
        }

        protected override IClient CreateClient(IConnection connection) => new Client(connection);
    }
}