using GameSpyLib.Database.Entity;
using GameSpyLib.Network;
using NetCoreServer;
using System.Net;

namespace Chat
{
    public class ChatServer : TemplateTcpServer
    {
        //we hard coded random key here for simplisity
        public static readonly string ClientKey = "0000000000000000";
       public static readonly string ServerKey = "0000000000000000";

        public ChatServer(IPAddress address, int port) : base(address, port)
        {
        }

        protected override TcpSession CreateSession()
        {
            return new ChatSession(this);
        }
    }
}
