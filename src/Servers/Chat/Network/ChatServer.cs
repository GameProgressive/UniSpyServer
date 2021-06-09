using NetCoreServer;
using System;
using System.Net;
using UniSpyLib.Network;

namespace Chat.Network
{
    internal sealed class ChatServer : UniSpyTCPServerBase
    {
        public ChatServer(Guid serverID, IPEndPoint endpoint) : base(serverID, endpoint)
        {
            SessionManager = new ChatSessionManager();
        }

        protected override TcpSession CreateSession() => new ChatSession(this);

    }
}
