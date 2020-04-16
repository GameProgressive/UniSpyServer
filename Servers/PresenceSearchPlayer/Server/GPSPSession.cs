using GameSpyLib.Network;
using PresenceSearchPlayer.Handler.CommandSwitcher;

namespace PresenceSearchPlayer
{
    public class GPSPSession : TemplateTcpSession
    {
        public GPSPSession(GPSPServer server) : base(server)
        {
        }

        protected override void OnReceived(string message)
        {
           new PSPCommandSwitcher().Switch(this, message);
        }
    }
}
