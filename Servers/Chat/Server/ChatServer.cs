using GameSpyLib.Database.Entity;
using GameSpyLib.Network;
using NetCoreServer;
using System.Net;

namespace Chat
{

    public class ChatServer : TemplateTcpServer
    {
        public static DatabaseEngine DB;
        public ChatServer(string serverName, DatabaseEngine engine, IPAddress address, int port) : base(serverName, address, port)
        {
            DB = engine;
        }
        protected override TcpSession CreateSession() { return new ChatSession(this); }
        protected override void Dispose(bool disposingManagedResources)
        {
            if (disposingManagedResources)
            { }
            base.Dispose(disposingManagedResources);
        }
    }
}

