using System;
using System.Net;
using UniSpyServer.Servers.QueryReport.Application;
using UniSpyServer.Servers.QueryReport.V2.Entity.Structure.Redis;
using UniSpyServer.Servers.QueryReport.V2.Handler.CmdHandler;
using UniSpyServer.UniSpyLib.Abstraction.Interface;
using UniSpyServer.UniSpyLib.Config;

namespace UniSpyServer.Servers.QueryReport.V2.Application
{
    internal sealed class UdpServer : UniSpyServer.UniSpyLib.Abstraction.BaseClass.Network.Udp.Server.UdpServer
    {
        public UdpServer(UniSpyServerConfig config) : base(config)
        {
            if (ClientMessageHandler.Channel is null)
            {
                ClientMessageHandler.Channel = new RedisChannel();
                ClientMessageHandler.Channel.StartSubscribe();
            }
        }
        protected override IClient CreateClient(IConnection connection) => new Client(connection);
    }
}