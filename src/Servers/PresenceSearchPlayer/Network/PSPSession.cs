using UniSpyLib.Network;
using PresenceSearchPlayer.Handler.CommandSwitcher;

namespace PresenceSearchPlayer.Network
{
    public class PSPSession : TCPSessionBase
    {
        public PSPSession(PSPServer server) : base(server)
        {
        }

        protected override void OnReceived(string message)
        {
            new PSPCommandSwitcher(this, message).Serialize();
        }
    }
}
