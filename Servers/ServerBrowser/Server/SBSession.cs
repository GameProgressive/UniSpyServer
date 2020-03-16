using GameSpyLib.Network;
using ServerBrowser.Handler.CommandSwitcher;
using GameSpyLib.Encryption;
namespace ServerBrowser
{
    public class SBSession : TemplateTcpSession
    {
        public byte[] EncXKey = new byte[261];
        public string Challenge;
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
