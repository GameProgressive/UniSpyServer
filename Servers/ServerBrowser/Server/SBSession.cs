using System.Collections.Generic;
using System.Linq;
using GameSpyLib.Encryption;
using GameSpyLib.Network;
using ServerBrowser.Entity.Structure.Packet.Request;
using ServerBrowser.Handler.CommandSwitcher;

namespace ServerBrowser
{
    public class SBSession : TemplateTcpSession
    {
        public GOACryptState EncState;
        public List<AdHocRequest> ServerMessageList;
        public SBSession(TemplateTcpServer server) : base(server)
        {
            ServerMessageList = new List<AdHocRequest>();
        }

        protected override void OnReceived(byte[] message)
        {
            new SBCommandSwitcher().Switch(this, message);
        }
    }
}
