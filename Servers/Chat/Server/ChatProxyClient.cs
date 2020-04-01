using GameSpyLib.Network;
namespace Chat.Server
{
    public class ChatProxyClient : TemplateTcpClient
    {
        private ChatSession _session;
        public ChatProxyClient(ChatSession session, string hostname, int port) : base("[proxy]", hostname, port)
        {
            _session = session;
        }

        public override void OnReceived(string message)
        {
            base.OnReceived(message);
            _session.SendAsync(message);
        }
    }
}
