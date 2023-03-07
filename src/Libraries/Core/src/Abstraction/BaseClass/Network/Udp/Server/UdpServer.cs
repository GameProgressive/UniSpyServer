using System;
using System.Linq;
using System.Net;
using UniSpy.Server.Core.Abstraction.Interface;
using UniSpy.Server.Core.Config;

namespace UniSpy.Server.Core.Abstraction.BaseClass.Network.Udp.Server
{
    /// <summary>
    /// This is a template class that helps creating a UDP Server with
    /// logging functionality and ServerName, as required in the old network stack.
    /// </summary>
    public abstract class UdpServer : NetCoreServer.UdpServer, IServer
    {
        public Guid ServerID { get; private set; }
        /// <summary>
        /// currently, we do not to care how to delete elements in dictionary
        /// </summary>
        public string ServerName { get; private set; }
        IPEndPoint IServer.ListeningIPEndPoint => (IPEndPoint)Endpoint;
        public IPEndPoint PublicIPEndPoint { get; private set; }
        public UdpServer(UniSpyServerConfig config) : base(config.ListeningIPEndPoint)
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
        protected abstract IClient CreateClient(IConnection connection);
        protected virtual IUdpConnection CreateConnection(IPEndPoint endPoint)
        {
            // we have to check if the endPoint is already in the dictionary,
            // which means the client is already in the dictionary, we do not need to create it
            // we just retrieve the connection from the dictionary
            var client = ClientManagerBase.GetClient(endPoint);
            if (client is null)
            {
                var connection = new UdpConnection(this, endPoint);
                client = CreateClient(connection);
                ClientManagerBase.AddClient(client);
            }

            return client.Connection as IUdpConnection;
        }
    }
}

