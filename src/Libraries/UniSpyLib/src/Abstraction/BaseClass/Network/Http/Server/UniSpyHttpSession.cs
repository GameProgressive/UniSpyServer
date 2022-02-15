using System.Net;
using NetCoreServer;
using UniSpyServer.UniSpyLib.Abstraction.Interface;
using UniSpyServer.UniSpyLib.Events;
using UniSpyServer.UniSpyLib.Logging;

namespace UniSpyServer.UniSpyLib.Abstraction.BaseClass.Network.Http.Server
{
    public abstract class UniSpyHttpSession : HttpSession, ISession
    {
        public EndPoint RemoteEndPoint => Socket.RemoteEndPoint;
        public IPEndPoint RemoteIPEndPoint => (IPEndPoint)RemoteEndPoint;
        public event OnConnectedEventHandler OnConnect;
        public event OnDisconnectedEventHandler OnDisconnect;
        public event OnReceivedEventHandler OnReceive;
        protected UniSpyHttpSession(UniSpyHttpServer server) : base(server)
        {
        }
        protected override void OnDisconnected()
        {
            OnDisconnect();
            base.OnDisconnected();
        }
        protected virtual void OnReceivedRequest(string message) { }
        protected override void OnReceivedRequest(HttpRequest request)
        {
            OnReceive(request);
            LogWriter.LogNetworkReceiving(RemoteIPEndPoint, request.Body);
            OnReceivedRequest(request.Body);
        }
        public bool Send(IResponse response)
        {
            response.Build();
            return base.SendResponseBodyAsync((string)response.SendingBuffer);
        }
        public bool BaseSend(IResponse response)
        {
            response.Build();
            return base.SendResponseBodyAsync((string)response.SendingBuffer);
        }
    }
}