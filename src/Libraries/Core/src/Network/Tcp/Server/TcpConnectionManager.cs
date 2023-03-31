using System;
using System.Net;
using UniSpy.Server.Core.Abstraction.Interface;
using UniSpy.Server.Core.Events;

namespace UniSpy.Server.Core.Network.Tcp.Server
{
    /// <summary>
    /// This is a template class that helps creating a TCP Server with logging functionality and ServerName, as required in the old network stack.
    /// </summary>
    public class TcpConnectionManager : NetCoreServer.TcpServer, IConnectionManager
    {
        public TcpConnectionManager(IPEndPoint endpoint) : base(endpoint)
        {
        }

        public event OnConnectingEventHandler OnInitialization;

        public new virtual void Start()
        {
            if (OptionSendBufferSize > int.MaxValue || OptionReceiveBufferSize > int.MaxValue)
            {
                throw new ArgumentException("Buffer size can not big than length of integer!");
            }
            base.Start();
        }
        protected override NetCoreServer.TcpSession CreateSession() => new TcpConnection(this);

        protected override void OnConnecting(NetCoreServer.TcpSession connection)
        {
            OnInitialization((IConnection)connection);
            base.OnConnecting(connection);
        }
    }
}
