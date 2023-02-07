using UniSpyServer.UniSpyLib.Abstraction.Interface;
using UniSpyServer.UniSpyLib.Config;

namespace UniSpyServer.Servers.WebServer.Application
{
    internal sealed class HttpServer : UniSpyLib.Abstraction.BaseClass.Network.Http.Server.HttpServer
    {
        public HttpServer(UniSpyServerConfig config) : base(config)
        {
        }

        protected override IClient CreateClient(IConnection connection) => new Client(connection);
    }
}