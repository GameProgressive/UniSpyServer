using PresenceSearchPlayer.Handler.CmdSwitcher;
using UniSpyLib.Network;
using UniSpyLib.Abstraction.BaseClass.Network.Tcp.Server;
namespace PresenceSearchPlayer.Network
{
    public sealed class Session : UniSpyTcpSession
    {
        public Session(Server server) : base(server)
        {
        }

        protected override void OnReceived(string message)
        {
            new CmdSwitcher(this, message).Switch();
        }
    }
}
