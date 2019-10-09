using GameSpyLib.Network;

namespace Chat
{
    public class ChatSession:TemplateTcpSession
    {
        public ChatSession(TemplateTcpServer server) : base(server)
        {
        }

        protected override void OnReceived(string message)
        {
            //LogWriter.Log.Write("[CHAT] Recv " + message, LogLevel.Error);
            //Stream.SendAsync("PING capricorn.goes.here :123456");
            //ChatHandler.Crypt(this, message);
        }
       
    }
}

