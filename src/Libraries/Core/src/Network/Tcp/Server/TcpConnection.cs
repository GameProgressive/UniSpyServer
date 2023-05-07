using System.Linq;
using System.Net;
using UniSpy.Server.Core.Abstraction.Interface;
using UniSpy.Server.Core.Encryption;
using UniSpy.Server.Core.Events;

namespace UniSpy.Server.Core.Network.Tcp.Server
{
    /// <summary>
    /// This is a template class that helps creating a TCP Session (formerly TCP stream)
    /// with logging functionality and ServerName, as required in the old network stack.
    /// </summary>
    public class TcpConnection : NetCoreServer.TcpSession, ITcpConnection
    {
        /// <summary>
        /// remote endpoint will dispose when disconnecting, however we need that in some situation,
        /// we must save it here for further use
        /// </summary>
        /// <value></value>
        public IPEndPoint RemoteIPEndPoint { get; private set; }
        IConnectionManager IConnection.Manager => (IConnectionManager)base.Server;
        public NetworkConnectionType ConnectionType => NetworkConnectionType.Tcp;
        public event OnConnectedEventHandler OnConnect;
        public event OnDisconnectedEventHandler OnDisconnect;
        public event OnReceivedEventHandler OnReceive;
        public TcpConnection(TcpConnectionManager server) : base(server)
        {
        }
        protected override void OnConnecting()
        {
            // we set ipendpoint here
            if (RemoteIPEndPoint is null)
            {
                RemoteIPEndPoint = (IPEndPoint)Socket.RemoteEndPoint;
            }
            base.OnConnecting();
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
        protected override void OnReceived(byte[] buffer, long offset, long size)
        {
            OnReceive(buffer.Skip((int)offset).Take((int)size).ToArray());
            base.OnReceived(buffer, offset, size);
        }
        void ITcpConnection.Disconnect() => Disconnect();
        public new void Send(string response) => Send(UniSpyEncoding.GetBytes(response));
        public new void Send(byte[] response) => base.SendAsync(response);

    }
}

