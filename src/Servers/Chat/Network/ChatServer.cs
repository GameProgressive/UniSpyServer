using NetCoreServer;
using System;
using System.Net;
using UniSpyLib.Abstraction.BaseClass.Network.Tcp.Server;
namespace Chat.Network
{
    internal sealed class ChatServer : UniSpyTcpServer
    {
        public ChatServer(Guid serverID, IPEndPoint endpoint) : base(serverID, endpoint)
        {
            SessionManager = new ChatSessionManager();
        }

        protected override TcpSession CreateSession() => new ChatSession(this);

    }
}
