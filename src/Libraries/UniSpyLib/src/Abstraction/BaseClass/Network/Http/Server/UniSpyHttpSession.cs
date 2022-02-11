using System.Net;
using NetCoreServer;
using UniSpyServer.UniSpyLib.Abstraction.Interface;
using UniSpyServer.UniSpyLib.Logging;

namespace UniSpyServer.UniSpyLib.Abstraction.BaseClass.Network.Http.Server
{
    public abstract class UniSpyHttpSession : HttpSession, ISession
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
        public bool Send(IResponse response)
        {
            response.Assemble();
            return base.SendResponseBodyAsync((string)response.SendingBuffer);
        }
        public bool BaseSend(IResponse response)
        {
            response.Assemble();
            return base.SendResponseBodyAsync((string)response.SendingBuffer);
        }
    }
}