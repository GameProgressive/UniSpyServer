using GameSpyLib.Network;

namespace Chat.Server
{
    public class ChatProxy : TemplateTcpProxy
    {
        /// <summary>
        /// we save session here
        /// </summary>
        private ChatSession _session;
        public ChatProxy(ChatSession session) : base()
        {
            _session = session;
            ConnectAsync();
        }

        protected override void OnReceived(string message)
        {
            base.OnReceived(message);
            if (message.Contains("NOTICE"))
            {
                return;
            }
            _session.SendAsync(message);
        }
    }
}
