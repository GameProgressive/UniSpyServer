using ServerBrowser.Entity.Structure.Misc;
using ServerBrowser.Entity.Structure.Request;
using ServerBrowser.Handler.CommandSwitcher;
using System.Collections.Generic;
using System.Linq;
using UniSpyLib.Abstraction.BaseClass.Network.Tcp.Server;
namespace ServerBrowser.Network
{
    public sealed class Session : UniSpyTcpSession
    {
        public string GameSecretKey { get; set; }
        public string ClientChallenge { get; set; }
        public List<AdHocRequest> ServerMessageList { get; set; }
        public EncryptionParameters EncParams { get; set; }

        public Session(UniSpyTcpServer server) : base(server)
        {
            ServerMessageList = new List<AdHocRequest>();
        }

        protected override void OnReceived(byte[] message) => new CmdSwitcher(this, message).Switch();
        protected override byte[] Encrypt(byte[] buffer)
        {
            SBEncryption enc;
            if (EncParams == null)
            {
                EncParams = new EncryptionParameters();
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
