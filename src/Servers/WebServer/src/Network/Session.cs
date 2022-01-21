using System;
using NetCoreServer;
using UniSpyServer.Servers.WebServer.Entity.Constant;
using UniSpyServer.Servers.WebServer.Handler;
using UniSpyServer.UniSpyLib.Abstraction.BaseClass;
using UniSpyServer.UniSpyLib.Abstraction.BaseClass.Network.Http.Server;
using UniSpyServer.UniSpyLib.Logging;

namespace UniSpyServer.Servers.WebServer.Network
{
    public sealed class Session : UniSpyHttpSession
    {
        public Session(UniSpyHttpServer server) : base(server)
        {
        }
        protected override void OnReceivedRequest(HttpRequest request)
        {
            if (request.BodyLength < 10)
            {
                //throw new UniSpyException("Request body length invalid");
            }
            var uri = new Uri(request.Url);

            if (!WebEndpoints.AvailableEndpoints.Contains(uri.LocalPath))
            {
                throw new UniSpyException($"Invalid webserver endpoint access: {uri.LocalPath}");
            }

            LogWriter.LogNetworkReceiving(RemoteIPEndPoint,
                              $"Url: {request.Url} " +
                              $"Headers: {request.Headers} " +
                              $"Cookies: {request.Cookies} " +
                              $"Protocol: {request.Protocol} " +
                              $"Content: {request.Body} ");

            new CmdSwitcher(this, request.Body).Switch();
        }
    }
}