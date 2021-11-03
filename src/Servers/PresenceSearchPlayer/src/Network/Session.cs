using UniSpyServer.Servers.PresenceSearchPlayer.Handler.CmdSwitcher;
using UniSpyServer.UniSpyLib.Network;
using UniSpyServer.UniSpyLib.Abstraction.BaseClass.Network.Tcp.Server;
namespace UniSpyServer.Servers.PresenceSearchPlayer.Network
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
