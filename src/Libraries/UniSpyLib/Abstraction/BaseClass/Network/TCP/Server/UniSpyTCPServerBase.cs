using NetCoreServer;
using Serilog.Events;
using System;
using System.Net;
using System.Net.Sockets;
using UniSpyLib.Abstraction.BaseClass;
using UniSpyLib.Abstraction.BaseClass.Network.TCP;
using UniSpyLib.Abstraction.Interface;
using UniSpyLib.Logging;

namespace UniSpyLib.Network
{
    /// <summary>
    /// This is a template class that helps creating a TCP Server with logging functionality and ServerName, as required in the old network stack.
    /// </summary>
    public abstract class UniSpyTCPServerBase : TcpServer, IUniSpyServer
    {
        public Guid ServerID { get; private set; }
        UniSpySessionManagerBase IUniSpyServer.SessionManager => SessionManager;
        public UniSpyTCPSessionManagerBase SessionManager { get; protected set; }
        /// <summary>
        /// Initialize TCP server with a given IP address and port number
        /// </summary>
        /// <param name="address">IP address</param>
        /// <param name="port">Port number</param>
        public UniSpyTCPServerBase(Guid serverID, IPEndPoint endpoint) : base(endpoint)
        {
            ServerID = serverID;
        }

        public override bool Start()
        {
            SessionManager.Start();
            return base.Start();
        }

        protected override void OnConnected(TcpSession session)
        {
            if (!SessionManager.SessionPool.ContainsKey(session.Id))
            {
                SessionManager.SessionPool.TryAdd(session.Id, (UniSpyTCPSessionBase)session);
            }
            base.OnConnected(session);
        }

        protected override void OnDisconnected(TcpSession session)
        {
            SessionManager.SessionPool.TryRemove(session.Id, out _);
            base.OnDisconnected(session);
        }

        /// <summary>
        /// Handle error notification
        /// </summary>
        /// <param name="error">Socket error code</param>
        protected override void OnError(SocketError error)
        {
            LogWriter.ToLog(LogEventLevel.Error, error.ToString());
        }
    }
}
