using Chat.Entity.Structure.ChatCommand;
using Chat.Handler.CommandHandler.CRYPT;
using Chat.Handler.CommandSwitcher;
using Chat.Handler.SystemHandler.ChatSessionManage;
using GameSpyLib.Network;
using NetCoreServer;
using System.Net;

namespace Chat.Server
{
    public class ChatServer : TemplateTcpServer
    {
        //we hard coded random key here for simplisity
        public static readonly string ClientKey = "0000000000000000";
        public static readonly string ServerKey = "0000000000000000";

        public static ChatCommandBaseManager CommandManager;
        public static ChatSessionManager SessionManager;

        public ChatServer(IPAddress address, int port) : base(address, port)
        {
            SessionManager = new ChatSessionManager();
            CommandManager = new ChatCommandBaseManager();
            //use this to add command into chatserver
            CommandManager.AddCommand(new CRYPT(), typeof(CRYPTHandler));
        }

        protected override TcpSession CreateSession()
        {
            //add sessions to session manager
            ChatSession session = new ChatSession(this);
            ChatSessionManager.Sessions.TryAdd(session.Id, session);
            return session;
        }
    }
}
