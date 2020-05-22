using System.Net;
using GameSpyLib.Network.Http.Server;
using NetCoreServer;

namespace WebServices.Server
{
    public class WebServer : TemplateHttpServer
    {
        public WebServer(IPEndPoint endpoint) : base(endpoint)
        {
        }

        protected override TcpSession CreateSession() { return new WebSession(this); }

        public WebServer(IPAddress address, int port) : base(address, port)
        {
        }


        public override bool Start()
        {
            //todo load service module here.
            return base.Start();
        }
    }
}
