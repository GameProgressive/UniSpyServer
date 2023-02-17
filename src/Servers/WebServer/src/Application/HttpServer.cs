using UniSpy.Server.Core.Abstraction.Interface;
using UniSpy.Server.Core.Config;

namespace UniSpy.Server.WebServer.Application
{
    internal sealed class HttpServer : UniSpy.Server.Core.Abstraction.BaseClass.Network.Http.Server.HttpServer
    {
        public HttpServer(UniSpyServerConfig config) : base(config)
        {
        }

        protected override IClient CreateClient(IConnection connection) => new Client(connection);
    }
}