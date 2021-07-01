using System;
using System.Net;
using NetCoreServer;
using UniSpyLib.Abstraction.Interface;

namespace UniSpyLib.Abstraction.BaseClass.Network.Http.Server
{
    public abstract class UniSpyHttpServer : HttpServer, IUniSpyServer
    {
        public Guid ServerID { get; private set; }
        public UniSpyHttpSessionManager SessionManager { get; protected set; }
        UniSpySessionManager IUniSpyServer.SessionManager => SessionManager;
        protected UniSpyHttpServer(Guid serverID, IPEndPoint endpoint) : base(endpoint)
        {
        }


        protected override void OnConnected(TcpSession session)
        {
            if (!SessionManager.SessionPool.ContainsKey(session.Id))
            {
                SessionManager.SessionPool.TryAdd(session.Id, (UniSpyHttpSession)session);
            }
            base.OnConnected(session);
        }

        protected override void OnDisconnected(TcpSession session)
        {
            SessionManager.SessionPool.TryRemove(session.Id, out _);
            base.OnDisconnected(session);
        }
    }
}