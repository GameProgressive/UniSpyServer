using UniSpyLib.Network;
using PresenceSearchPlayer.Handler.CommandSwitcher;

namespace PresenceSearchPlayer
{
    public class PSPSession : TemplateTcpSession
    {
        public PSPSession(PSPServer server) : base(server)
        {
        }

        protected override void OnReceived(string message)
        {
            PSPCommandSwitcher.Switch(this, message);
        }
    }
}
