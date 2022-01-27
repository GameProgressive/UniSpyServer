using UniSpyServer.Servers.ServerBrowser.Entity.Structure.Misc;
using UniSpyServer.Servers.ServerBrowser.Entity.Structure.Request;
using UniSpyServer.Servers.ServerBrowser.Handler.CommandSwitcher;
using System.Collections.Generic;
using System.Linq;
using UniSpyServer.UniSpyLib.Abstraction.BaseClass.Network.Tcp.Server;
namespace UniSpyServer.Servers.ServerBrowser.Network
{
    public sealed class Session : UniSpyTcpSession
    {
        public string GameSecretKey { get; set; }
        public string ClientChallenge { get; set; }
        public List<ServerInfoRequest> ServerMessageStack { get; set; }
        public EncryptionParameters EncParams { get; set; }

        public Session(UniSpyTcpServer server) : base(server)
        {
            ServerMessageStack = new List<ServerInfoRequest>();
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
            var cryptHeader = buffer.Take(14).ToArray();
            var cipherBody = enc.Encrypt(buffer.Skip(14).ToArray());
            return cryptHeader.Concat(cipherBody).ToArray();
        }
    }
}
