using NetCoreServer;
using UniSpyLib.Abstraction.BaseClass.Network.Http.Server;

namespace WebServer.Network
{
    internal class WebSession : UniSpyHttpSession
    {
        public WebSession(UniSpyHttpServer server) : base(server)
        {
        }
        protected override void OnReceivedRequest(HttpRequest request)
        {
            
        }
    }
}