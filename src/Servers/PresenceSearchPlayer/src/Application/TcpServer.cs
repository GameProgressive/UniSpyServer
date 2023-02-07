using System;
using System.Net;
using UniSpyServer.Servers.PresenceSearchPlayer.Entity.Structure;
using UniSpyServer.UniSpyLib.Abstraction.Interface;
using UniSpyServer.UniSpyLib.Config;

namespace UniSpyServer.Servers.PresenceSearchPlayer.Application
{
    internal sealed class TcpServer : UniSpyServer.UniSpyLib.Abstraction.BaseClass.Network.Tcp.Server.TcpServer
    {
        public TcpServer(UniSpyServerConfig config) : base(config)
        {
        }
        protected override IClient CreateClient(IConnection connection) => new Client(connection);
    }
}