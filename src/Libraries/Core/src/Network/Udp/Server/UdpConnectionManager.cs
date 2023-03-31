using System;
using System.Linq;
using System.Net;
using UniSpy.Server.Core.Abstraction.Interface;
using UniSpy.Server.Core.Events;

namespace UniSpy.Server.Core.Network.Udp.Server
{
    /// <summary>
    /// This is a template class that helps creating a UDP Server with
    /// logging functionality and ServerName, as required in the old network stack.
    /// </summary>
    public class UdpConnectionManager : NetCoreServer.UdpServer, IConnectionManager
    {
        public UdpConnectionManager(IPEndPoint endpoint) : base(endpoint)
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
        protected override void OnStarted() => ReceiveAsync();

        /// <summary>
        /// Send unencrypted data
        /// </summary>
        /// <param name="buffer">plaintext</param>
        /// <returns>is sending succeed</returns>
        protected override void OnReceived(EndPoint endPoint, byte[] buffer, long offset, long size)
        {
            var connection = CreateConnection((IPEndPoint)endPoint);
            (connection as UdpConnection).OnReceived(buffer.Skip((int)offset).Take((int)size).ToArray());
        }
        protected virtual IUdpConnection CreateConnection(IPEndPoint endPoint)
        {
            var connection = new UdpConnection(this, endPoint);
            var client = OnInitialization(connection);
            return client.Connection as IUdpConnection;
        }
    }
}

