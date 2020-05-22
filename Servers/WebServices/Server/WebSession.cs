using System;
using GameSpyLib.Network.Http.Server;
using NetCoreServer;

namespace WebServices.Server
{
    public class WebSession : TemplateHttpSession
    {
        public WebSession(HttpServer server) : base(server)
        {
        }

        protected override void OnReceivedPostRequest(HttpRequest request)
        {
            throw new NotImplementedException();
        }
    }
}
