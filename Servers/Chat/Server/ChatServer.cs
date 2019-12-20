using GameSpyLib.Database;
using GameSpyLib.Network;
using NetCoreServer;
using System.Net;

namespace Chat
{

    public class ChatServer : TemplateTcpServer
    {
        public static DatabaseDriver DB;
        public ChatServer(string serverName, DatabaseDriver databaseDriver, IPAddress address, int port) : base(serverName, address, port)
        {
            DB = databaseDriver;
        }
        protected override TcpSession CreateSession() { return new ChatSession(this); }
        protected override void Dispose(bool disposingManagedResources)
        {
            if (disposingManagedResources)
            { }
            DB?.Close();
            DB?.Dispose();
            base.Dispose(disposingManagedResources);
        }
    }
}

