using GameSpyLib.Common;
using GameSpyLib.Network;
using GameSpyLib.RetroSpyConfig;
using System.Linq;

namespace Chat.Server
{
    public class ChatProxy : TemplateTcpClient
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
            _session.SendAsync(message);
        }
    }
}
