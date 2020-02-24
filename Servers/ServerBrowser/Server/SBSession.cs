using GameSpyLib.Network;
using ServerBrowser.Handler.CommandSwitcher;

namespace ServerBrowser
{
    public class SBSession : TemplateTcpSession
    {
        public SBSession(TemplateTcpServer server) : base(server)
        {
        }

        protected override void OnReceived(string message)
        {
            CommandSwitcher.Switch(this, message);
        }
        protected override void OnReceived(byte[] buffer, long offset, long size)
        {
            base.OnReceived(buffer, offset, size);
        }
    }
}
