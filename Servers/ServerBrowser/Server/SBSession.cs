using GameSpyLib.Network;
using ServerBrowser.Handler.CommandSwitcher;
using GameSpyLib.Encryption;
using System;

namespace ServerBrowser
{
    public class SBSession : TemplateTcpSession
    {
        public GOACryptState EncState;

        public SBSession(TemplateTcpServer server) : base(server)
        {
        }

        protected override void OnReceived(byte[] message)
        {
            CommandSwitcher.Switch(this, message);
        }
    }
}
