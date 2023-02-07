using System;
using System.Linq;
using System.Net;
using UniSpyServer.UniSpyLib.Abstraction.Interface;
using UniSpyServer.UniSpyLib.Config;

namespace UniSpyServer.Servers.GameStatus.Application
{
    internal sealed class TcpServer : UniSpyServer.UniSpyLib.Abstraction.BaseClass.Network.Tcp.Server.TcpServer
    {
        public TcpServer(UniSpyServerConfig config) : base(config)
        {
        }
        
        protected override IClient CreateClient(IConnection connection) => new Client(connection);
    }
}