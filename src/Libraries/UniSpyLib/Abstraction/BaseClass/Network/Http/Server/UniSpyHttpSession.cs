using System.Net;
using NetCoreServer;
using UniSpyLib.Abstraction.Interface;
using UniSpyLib.Logging;

namespace UniSpyLib.Abstraction.BaseClass.Network.Http.Server
{
    public abstract class UniSpyHttpSession : HttpSession, IUniSpySession
    {
        protected UniSpyHttpSession(UniSpyHttpServer server) : base(server)
        {
        }
        public EndPoint RemoteEndPoint => Socket.RemoteEndPoint;
        public IPEndPoint RemoteIPEndPoint => (IPEndPoint)RemoteEndPoint;
        protected virtual void OnReceivedRequest(string message) { }
        protected override void OnReceivedRequest(HttpRequest request)
        {
            LogWriter.LogNetworkReceiving(RemoteIPEndPoint, request.Body);
            OnReceivedRequest(request.Body);
        }
        public bool Send(IUniSpyResponse response)
        {
            response.Build();
            return base.SendResponseBodyAsync((string)response.SendingBuffer);
        }
        public bool BaseSend(IUniSpyResponse response)
        {
            response.Build();
            return base.SendResponseBodyAsync((string)response.SendingBuffer);
        }
    }
}