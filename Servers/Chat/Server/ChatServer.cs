using Chat.Entity.Structure.ChatCommand;
using Chat.Handler.CommandHandler;
using Chat.Handler.CommandSwitcher;
using Chat.Handler.SystemHandler.ChannelManage;
using Chat.Handler.SystemHandler.ChatCommandManage;
using Chat.Handler.SystemHandler.ChatSessionManage;
using GameSpyLib.Network;
using NetCoreServer;
using System.Collections.Generic;
using System.Net;

namespace Chat.Server
{
    public class ChatServer : TemplateTcpServer
    {
        public static readonly string ServerDomain = "www.rspy.cc";
        //we hard coded random key here for simplisity
        public static readonly string ClientKey = "0000000000000000";
        public static readonly string ServerKey = "0000000000000000";

        public static ChatCommandManager CommandManager;
        public static ChatSessionManager SessionManager;
        public static ChatChannelManager ChannelManager;

        public ChatServer(IPAddress address, int port) : base(address, port)
        {
            SessionManager = new ChatSessionManager();
            CommandManager = new ChatCommandManager();
            ChannelManager = new ChatChannelManager();
            //use this to add command into chatserver
            CommandManager.Start();
            ChannelManager.Start();
            SessionManager.Start();
        }

        protected override TcpSession CreateSession()
        {
            //add sessions to session manager
            ChatSession session = new ChatSession(this);
            ChatSessionManager.AddSession(session);
            return session;
        }
    }
}
