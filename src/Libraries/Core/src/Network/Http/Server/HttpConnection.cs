using System.Linq;
using System.Net;
using UniSpy.Server.Core.Abstraction.Interface;
using UniSpy.Server.Core.Encryption;
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
        private HttpBufferCache _bufferCache = new HttpBufferCache();
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
        protected override void OnReceived(byte[] buffer, long offset, long size)
        {
            var req = UniSpyEncoding.GetString(buffer.Take((int)size).ToArray());
            string compeleteBuffer;
            if (_bufferCache.ProcessBuffer(req, out compeleteBuffer))
            {
                var completeBytes = UniSpyEncoding.GetBytes(compeleteBuffer);
                base.OnReceived(completeBytes, offset, completeBytes.Length);
            }
        }
        protected override void OnReceivedRequest(NetCoreServer.HttpRequest request) => OnReceive(new HttpRequest(request));
        void IConnection.Send(string response)
        {
            // Response.MakeOkResponse();
            Response.SetBegin(200);
            Response.SetBody(response);
            base.SendResponse();
        }

        void IConnection.Send(byte[] response)
        {
            // Response.MakeOkResponse();
            Response.SetBegin(200);
            Response.SetBody(response);
            base.SendResponse();
        }

        void ITcpConnection.Disconnect() => Disconnect();

    }
}