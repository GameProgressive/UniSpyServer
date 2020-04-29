using GameSpyLib.Encryption;
using GameSpyLib.Network;
using ServerBrowser.Handler.CommandSwitcher;

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
            new SBCommandSwitcher().Switch(this, message);
        }
    }
}
