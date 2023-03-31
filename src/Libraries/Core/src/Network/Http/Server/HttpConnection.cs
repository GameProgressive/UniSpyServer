using System.Net;
using UniSpy.Server.Core.Abstraction.Interface;
using UniSpy.Server.Core.Events;

namespace UniSpy.Server.Core.Network.Http.Server
{
    public class HttpConnection : NetCoreServer.HttpSession, IHttpConnection
    {
        public IPEndPoint RemoteIPEndPoint { get; private set; }
        public NetworkConnectionType ConnectionType => NetworkConnectionType.Http;
        public IConnectionManager Manager => (IConnectionManager)Server;

        public event OnConnectedEventHandler OnConnect;
        public event OnDisconnectedEventHandler OnDisconnect;
        public event OnReceivedEventHandler OnReceive;
        public HttpConnection(HttpConnectionManager server) : base(server)
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