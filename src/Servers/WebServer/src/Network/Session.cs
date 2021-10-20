using System;
using NetCoreServer;
using UniSpyServer.UniSpyLib.Abstraction.BaseClass;
using UniSpyServer.UniSpyLib.Abstraction.BaseClass.Network.Http.Server;
using UniSpyServer.UniSpyLib.Logging;
using UniSpyServer.WebServer.Entity.Constant;
using UniSpyServer.WebServer.Handler;

namespace UniSpyServer.WebServer.Network
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
                              $"\tUrl: {request.Url}\n" +
                              $"\tHeaders: {request.Headers}\n" +
                              $"\tCookies: {request.Cookies}\n" +
                              $"\tProtocol: {request.Protocol}\n" +
                              $"\tBody length: {request.BodyLength}"
            );

            new CmdSwitcher(this, request.Body).Switch();
        }
    }
}