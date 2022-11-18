using System.Net;
using UniSpyServer.UniSpyLib.Abstraction.Interface;
using UniSpyServer.UniSpyLib.Events;

namespace UniSpyServer.UniSpyLib.Abstraction.BaseClass.Network.Http.Server
{
    public class HttpConnection : NetCoreServer.HttpSession, IHttpConnection
    {
        public IPEndPoint RemoteIPEndPoint { get; private set; }
        IServer IConnection.Server => (HttpServer)Server;
        public NetworkConnectionType ConnectionType => NetworkConnectionType.Http;
        public event OnConnectedEventHandler OnConnect;
        public event OnDisconnectedEventHandler OnDisconnect;
        public event OnReceivedEventHandler OnReceive;
        public HttpConnection(HttpServer server) : base(server)
        {
        }
        protected override void OnConnecting()
        {
            if (RemoteIPEndPoint is null)
            {
                RemoteIPEndPoint = (IPEndPoint)Socket.RemoteEndPoint;
            }
            ((HttpServer)Server).CreateClient(this);
            base.OnConnecting();
        }
        protected override void OnConnected() => OnConnect();
        protected override void OnDisconnected() => OnDisconnect();
        protected override void OnReceivedRequest(NetCoreServer.HttpRequest request) => OnReceive(new HttpRequest(request));
        void IConnection.Send(string response)
        {
            // Response.MakeOkResponse();
            Response.SetBegin(200);
            Response.SetBody(response);
            base.SendResponseAsync();
        }

        void IConnection.Send(byte[] response)
        {
            // Response.MakeOkResponse();
            Response.SetBegin(200);
            Response.SetBody(response);
            base.SendResponseAsync();
        }

        void ITcpConnection.Disconnect() => Disconnect();

    }
}