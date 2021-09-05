using PresenceSearchPlayer.Handler.CmdSwitcher;
using UniSpyLib.Network;
using UniSpyLib.Abstraction.BaseClass.Network.Tcp.Server;
namespace PresenceSearchPlayer.Network
{
    public sealed class PSPSession : UniSpyTcpSession
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
