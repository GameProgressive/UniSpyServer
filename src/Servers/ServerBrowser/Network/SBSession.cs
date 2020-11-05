using UniSpyLib.Encryption;
using UniSpyLib.Network;
using ServerBrowser.Entity.Structure.Packet.Request;
using ServerBrowser.Handler.CommandSwitcher;
using System.Collections.Generic;

namespace ServerBrowser.Network
{
    public class SBSession : TCPSessionBase
    {
        public GOACryptState EncState;
        public List<AdHocRequest> ServerMessageList;
        public SBSession(TCPServerBase server) : base(server)
        {
            ServerMessageList = new List<AdHocRequest>();
        }

        protected override void OnReceived(byte[] message)
        {
            SBCommandSwitcher.Switch(this, message);
        }
    }
}
