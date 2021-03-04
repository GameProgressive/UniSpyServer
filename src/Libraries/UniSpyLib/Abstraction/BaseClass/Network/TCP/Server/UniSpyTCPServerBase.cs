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
            SessionManager = new UniSpyTCPSessionManagerBase();
        }

        protected override void OnConnected(TcpSession session)
        {
            if (!SessionManager.Sessions.ContainsKey(session.Id))
            {
                SessionManager.Sessions.TryAdd(session.Id, (UniSpyTCPSessionBase)session);
            }
            //LogWriter.ToLog(LogEventLevel.Information, $"[Conn] IP:{session.Socket.RemoteEndPoint}");
            base.OnConnected(session);
        }

        protected override void OnDisconnected(TcpSession session)
        {
            SessionManager.Sessions.TryRemove(session.Id, out _);
            //We create our own RemoteEndPoint because when client disconnect,
            //the session socket will dispose immidiatly
            //LogWriter.ToLog(LogEventLevel.Information, $"[Disc] IP:{((UniSpyTCPSessionBase)session).RemoteEndPoint}");
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
