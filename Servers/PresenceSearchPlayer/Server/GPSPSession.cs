using GameSpyLib.Common;
using GameSpyLib.MiscMethod;
using GameSpyLib.Network;
using PresenceSearchPlayer.Enumerator;
using PresenceSearchPlayer.Handler.CommandHandler;
using System.Collections.Generic;

namespace PresenceSearchPlayer
{
    public class GPSPSession : TemplateTcpSession
    {
        public GPSPSession(GPSPServer server) : base(server)
        {
        }

        protected override void OnReceived(string message)
        {
            CommandSwitcher.Switch(this, message);
        }
    }
}
