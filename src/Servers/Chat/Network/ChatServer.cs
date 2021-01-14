using Chat.Handler.SystemHandler.ChannelManage;
using Chat.Handler.SystemHandler.ChatSessionManage;
using NetCoreServer;
using System.Net;
using UniSpyLib.Network;

namespace Chat.Network
{
    public class ChatServer : TCPServerBase
    {
        public const string ServerDomain = "rspy.cc";
        //we hard coded random key here for simplisity
        public static readonly string ClientKey = "0000000000000000";
        public static readonly string ServerKey = "0000000000000000";

        //public static ChatCommandManager CommandManager;
        public static ChatSessionManager SessionManager;
        public static ChatChannelManager ChannelManager;

        public ChatServer(IPAddress address, int port) : base(address, port)
        {
            SessionManager = new ChatSessionManager();
            ChannelManager = new ChatChannelManager();
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
