using System.Net;
using UniSpyServer.UniSpyLib.Abstraction.Interface;
using UniSpyServer.UniSpyLib.Events;

namespace UniSpyServer.UniSpyLib.Application.Network.Http.Server
{
    public class HttpSession : NetCoreServer.HttpSession, IHttpSession
    {
        public IPEndPoint RemoteIPEndPoint { get; private set; }
        IServer ISession.Server => (HttpServer)Server;
        public NetworkConnectionType ConnectionType => NetworkConnectionType.Http;
        public event OnConnectedEventHandler OnConnect;
        public event OnDisconnectedEventHandler OnDisconnect;
        public event OnReceivedEventHandler OnReceive;
        public HttpSession(HttpServer server) : base(server)
        {
        }
        protected override void OnConnecting()
        {
            if (RemoteIPEndPoint is null)
            {
                RemoteIPEndPoint = (IPEndPoint)Socket.RemoteEndPoint;
            }
            base.OnConnecting();
        }
        protected override void OnConnected() => OnConnect();
        protected override void OnDisconnected() => OnDisconnect();
        protected override void OnReceivedRequest(NetCoreServer.HttpRequest request) => OnReceive(new HttpRequest(request));
        void ISession.Send(string response)
        {
            // Response.MakeOkResponse();
            Response.SetBegin(200);
            Response.SetBody(response);
            base.SendResponseAsync();
        }

        void ISession.Send(byte[] response)
        {
            // Response.MakeOkResponse();
            Response.SetBegin(200);
            Response.SetBody(response);
            base.SendResponseAsync();
        }

        void ITcpSession.Disconnect() => Disconnect();

    }
}