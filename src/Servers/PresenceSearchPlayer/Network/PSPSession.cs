using UniSpyLib.Network;
using PresenceSearchPlayer.Handler.CmdSwitcher;

namespace PresenceSearchPlayer.Network
{
    public class PSPSession : TCPSessionBase
    {
        public PSPSession(PSPServer server) : base(server)
        {
        }

        protected override void OnReceived(string message)
        {
            new PSPCmdSwitcher(this, message).Switch();
        }
    }
}
