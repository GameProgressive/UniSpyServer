﻿using NetCoreServer;
using System;
using System.Net;
using UniSpyServer.UniSpyLib.Abstraction.BaseClass.Network.Tcp.Server;

namespace UniSpyServer.GameStatus.Network
{
    public sealed class Server : UniSpyTcpServer
    {
        public Server(Guid serverID, IPEndPoint endpoint) : base(serverID, endpoint)
        {
            SessionManager = new SessionManager();
        }

        protected override TcpSession CreateSession() => new Session(this);
    }
}
