using UniSpyLib.Encryption;
using UniSpyLib.Network;
using ServerBrowser.Handler.CommandSwitcher;
using System.Collections.Generic;
using ServerBrowser.Entity.Structure.Request;

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
            new SBCommandSwitcher(this, message).Switch();
        }
    }
}
