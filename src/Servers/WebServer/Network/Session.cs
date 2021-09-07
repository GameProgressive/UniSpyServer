using NetCoreServer;
using UniSpyLib.Abstraction.BaseClass.Network.Http.Server;

namespace WebServer.Network
{
    internal class Session : UniSpyHttpSession
    {
        public Session(UniSpyHttpServer server) : base(server)
        {
        }
        protected override void OnReceivedRequest(HttpRequest request)
        {
            
        }
    }
}