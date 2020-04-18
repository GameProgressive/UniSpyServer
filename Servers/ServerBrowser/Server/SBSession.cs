using GameSpyLib.Encryption;
using GameSpyLib.Network;
using NatNegotiation.Entity.Structure.Packet;
using ServerBrowser.Handler.CommandSwitcher;
using System.Linq;

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
            if (message.Take(6).SequenceEqual(BasePacket.MagicData))
            {
                new NatNegCommandSwitcher().Switch(this, message);
            }
            else
            {
                new SBCommandSwitcher().Switch(this, message);
            }
        }
    }
}
