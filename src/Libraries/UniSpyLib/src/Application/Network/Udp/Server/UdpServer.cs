using System;
using System.Net;
using System.Reflection;
using NetCoreServer;
using UniSpyServer.UniSpyLib.Abstraction.BaseClass;
using UniSpyServer.UniSpyLib.Abstraction.Interface;

namespace UniSpyServer.UniSpyLib.Application.Network.Udp.Server
{
    /// <summary>
    /// This is a template class that helps creating a UDP Server with
    /// logging functionality and ServerName, as required in the old network stack.
    /// </summary>
    public class UdpServer : NetCoreServer.UdpServer, IServer
    {
        public Guid ServerID { get; private set; }
        /// <summary>
        /// currently, we do not to care how to delete elements in dictionary
        /// </summary>
        public string ServerName { get; private set; }

        public UdpServer(Guid serverID, string serverName, IPEndPoint endpoint) : base(endpoint)
        {
            ServerID = serverID;
            ServerName = serverName;
        }
        public override bool Start()
        {
            //可以将server中的事件绑定到CmdSwitcher中
            if (OptionSendBufferSize > int.MaxValue || OptionReceiveBufferSize > int.MaxValue)
            {
                throw new ArgumentException("Buffer size can not big than length of integer!");
            }
            // SessionManager.Start();
            ReceiveAsync();
            return base.Start();
        }

        protected override void OnStarted() => ReceiveAsync();
        protected UdpSession CreateSession(EndPoint endPoint)
        {
            var session = new UdpSession(this, endPoint);
            ClientBase.CreateClient(session);
            return session;
        }
        /// <summary>
        /// Continue receive datagrams
        /// </summary>
        /// <param name="endpoint"></param>
        /// <param name="sent"></param>
        protected override void OnSent(EndPoint endpoint, long sent) => ReceiveAsync();
        /// <summary>
        /// Send unencrypted data
        /// </summary>
        /// <param name="buffer">plaintext</param>
        /// <returns>is sending succeed</returns>
        protected override void OnReceived(EndPoint endPoint, byte[] buffer, long offset, long size)
        {
            var session = CreateSession(endPoint);
            // WAINING!!!!!!: Do not change the sequence of ReceiveAsync()

            ReceiveAsync();
            // session.OnReceived(plainText);
            session.OnReceived(buffer);
        }
    }
}
