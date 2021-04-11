using ServerBrowser.Entity.Structure.Misc;
using ServerBrowser.Entity.Structure.Request;
using ServerBrowser.Handler.CommandSwitcher;
using System.Collections.Generic;
using System.Linq;
using UniSpyLib.Network;

namespace ServerBrowser.Network
{
    internal sealed class SBSession : UniSpyTCPSessionBase
    {
        public string GameSecretKey { get; set; }
        public string ClientChallenge { get; set; }
        public List<AdHocRequest> ServerMessageList { get; set; }
        public SBEncryptionParameters EncParams { get; set; }

        public SBSession(UniSpyTCPServerBase server) : base(server)
        {
            ServerMessageList = new List<AdHocRequest>();
        }

        protected override void OnReceived(byte[] message) => new SBCmdSwitcher(this, message).Switch();
        protected override byte[] Encrypt(byte[] buffer)
        {
            SBEncryption enc;
            if (EncParams == null)
            {
                EncParams = new SBEncryptionParameters();
                enc = new SBEncryption(
                GameSecretKey,
                ClientChallenge,
                EncParams);
            }
            else
            {
                enc = new SBEncryption(EncParams);
            }
            var cryptHeader = buffer.Take(14);
            var cipherBody = enc.Encrypt(buffer.Skip(14).ToArray());
            return cryptHeader.Concat(cipherBody).ToArray();
        }
    }
}
