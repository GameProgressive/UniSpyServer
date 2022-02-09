using System;
using System.Net;
using NetCoreServer;
using UniSpyServer.UniSpyLib.Abstraction.Interface;

namespace UniSpyServer.UniSpyLib.Abstraction.BaseClass.Network.Tcp.Server
{
    /// <summary>
    /// This is a template class that helps creating a TCP Server with logging functionality and ServerName, as required in the old network stack.
    /// </summary>
    public abstract class UniSpyTcpServer : TcpServer, IServer
    {
        public Guid ServerID { get; private set; }
        SessionManager IServer.SessionManager => SessionManager;
        public UniSpyTcpSessionManager SessionManager { get; protected set; }
        /// <summary>
        /// Initialize TCP server with a given IP address and port number
        /// </summary>
        /// <param name="address">IP address</param>
        /// <param name="port">Port number</param>
        public UniSpyTcpServer(Guid serverID, IPEndPoint endpoint) : base(endpoint)
        {
            ServerID = serverID;
        }

        public override bool Start()
        {
            if (OptionSendBufferSize > int.MaxValue || OptionReceiveBufferSize > int.MaxValue)
            {
                throw new ArgumentException("Buffer size can not big than length of integer!");
            }
            SessionManager.Start();
            return base.Start();
        }

        protected override void OnConnected(TcpSession session)
        {
            if (!SessionManager.SessionPool.ContainsKey(session.Id))
            {
                SessionManager.SessionPool.Add(session.Id, (UniSpyTcpSession)session);
            }
            base.OnConnected(session);
        }

        protected override void OnDisconnected(TcpSession session)
        {
            SessionManager.SessionPool.Remove(session.Id);
            base.OnDisconnected(session);
        }
    }
}
