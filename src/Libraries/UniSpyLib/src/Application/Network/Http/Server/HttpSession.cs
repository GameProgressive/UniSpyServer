using System.Net;
using NetCoreServer;
using UniSpyServer.UniSpyLib.Abstraction.Interface;
using UniSpyServer.UniSpyLib.Events;
using UniSpyServer.UniSpyLib.Logging;

namespace UniSpyServer.UniSpyLib.Application.Network.Http.Server
{
    public class HttpSession : NetCoreServer.HttpSession, ISession
    {
        public IPEndPoint RemoteIPEndPoint => (IPEndPoint)Socket.RemoteEndPoint;
        IServer ISession.Server => (HttpServer)Server;
        public event OnConnectedEventHandler OnConnect;
        public event OnDisconnectedEventHandler OnDisconnect;
        public event OnReceivedEventHandler OnReceive;
        public HttpSession(HttpServer server) : base(server)
        {
        }
        protected override void OnConnected()
        {
            OnConnect();
            base.OnConnected();
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
        void ISession.Send(string response)
        {
            base.SendResponseBodyAsync(response);
        }

        void ISession.Send(byte[] response)
        {
            base.SendResponseBodyAsync(response);
        }
    }
}