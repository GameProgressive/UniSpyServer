using System;
using System.Net;
using NetCoreServer;
using UniSpyServer.UniSpyLib.Abstraction.Interface;

namespace UniSpyServer.UniSpyLib.Abstraction.BaseClass.Network.Http.Server
{
    public abstract class UniSpyHttpServer : HttpServer, IServer
    {
        public Guid ServerID { get; private set; }
        public UniSpyHttpSessionManager SessionManager { get; protected set; }
        SessionManager IServer.SessionManager => SessionManager;
        protected UniSpyHttpServer(Guid serverID, IPEndPoint endpoint) : base(endpoint)
        {
        }


        protected override void OnConnected(TcpSession session)
        {
            if (!SessionManager.SessionPool.ContainsKey(session.Id))
            {
                SessionManager.SessionPool.Add(session.Id, (UniSpyHttpSession)session);
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