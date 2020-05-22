using System;
using System.Net;
using System.Net.Sockets;
using GameSpyLib.Logging;
using NetCoreServer;
using Serilog.Events;

namespace GameSpyLib.Network.Http.Server
{
    public abstract class TemplateHttpServer : HttpServer
    {
        public TemplateHttpServer(IPEndPoint endpoint) : base(endpoint)
        {
        }

        public TemplateHttpServer(IPAddress address, int port) : base(address, port)
        {
        }

        protected override void OnError(SocketError error)
        {
            base.OnError(error);
            LogWriter.ToLog(LogEventLevel.Error, error.ToString());
        }
    }
}
