using ServerBrowser.Entity.Structure.Request;
using ServerBrowser.Handler.CommandSwitcher;
using System.Collections.Generic;
using UniSpyLib.Encryption;
using UniSpyLib.Network;

namespace ServerBrowser.Network
{
    public class SBSession : UniSpyTCPSessionBase
    {
        public GOACryptState EncState { get; set; }
        public List<AdHocRequest> ServerMessageList { get; set; }
        public SBSession(UniSpyTCPServerBase server) : base(server)
        {
            ServerMessageList = new List<AdHocRequest>();
        }

        protected override void OnReceived(byte[] message)
        {
            new SBCmdSwitcher(this, message).Switch();
        }
    }
}
