using System;
using System.Net;
using UniSpy.Server.Core.Abstraction.Interface;
using UniSpy.Server.Core.Config;

namespace UniSpy.Server.Core.Abstraction.BaseClass.Network.Tcp.Server
{
    /// <summary>
    /// This is a template class that helps creating a TCP Server with logging functionality and ServerName, as required in the old network stack.
    /// </summary>
    public abstract class TcpServer : NetCoreServer.TcpServer, IServer
    {
        public Guid ServerID { get; private set; }
        /// <summary>
        /// Initialize TCP server with a given IP address and port number
        /// </summary>
        /// <param name="address">IP address</param>
        /// <param name="port">Port number</param>
        public string ServerName { get; private set; }

        IPEndPoint IServer.ListeningIPEndPoint => (IPEndPoint)Endpoint;

        public IPEndPoint PublicIPEndPoint { get; private set; }

        public TcpServer(UniSpyServerConfig config) : base(config.ListeningIPEndPoint)
        {
            ServerID = config.ServerID;
            ServerName = config.ServerName;
            PublicIPEndPoint = config.PublicIPEndPoint;
        }

        public new virtual void Start()
        {
            if (OptionSendBufferSize > int.MaxValue || OptionReceiveBufferSize > int.MaxValue)
            {
                throw new ArgumentException("Buffer size can not big than length of integer!");
            }
            base.Start();
        }
        protected abstract IClient CreateClient(IConnection connection);

        protected override NetCoreServer.TcpSession CreateSession() => new TcpConnection(this);

        protected override void OnConnecting(NetCoreServer.TcpSession connection)
        {
            base.OnConnecting(connection);
            CreateClient((IConnection)connection);
        }
    }
}
