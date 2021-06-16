using PresenceSearchPlayer.Handler.CmdSwitcher;
using UniSpyLib.Network;

namespace PresenceSearchPlayer.Network
{
    public sealed class PSPSession : UniSpyTCPSessionBase
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
