using GameSpyLib.Network;

namespace ServerBrowser
{
    public class SBSession : TemplateTcpSession
    {
        public SBSession(TemplateTcpServer server) : base(server)
        {
        }

        protected override void OnReceived(string message)
        {
            base.OnReceived(message);
            Send(@"1111");
        }
    }
}
