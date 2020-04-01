using GameSpyLib.Database.Entity;
using GameSpyLib.Network;
using NetCoreServer;
using System.Net;

namespace Chat
{
    public class ChatServer : TemplateTcpServer
    {
        public ChatServer(IPAddress address, int port) : base(address, port)
        {
        }

        protected override TcpSession CreateSession()
        {
            return new ChatSession(this);
        }

        protected override void Dispose(bool disposingManagedResources)
        {
            if (disposingManagedResources)
            {
            }

            base.Dispose(disposingManagedResources);
        }
    }
}
