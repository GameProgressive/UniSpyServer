using UniSpyServer.Servers.GameStatus.Entity.Structure.Misc;
using UniSpyServer.UniSpyLib.Abstraction.BaseClass;
using UniSpyServer.UniSpyLib.Abstraction.Interface;
using UniSpyServer.UniSpyLib.Encryption;
using UniSpyServer.UniSpyLib.Logging;

namespace UniSpyServer.Servers.GameStatus.Entity.Structure
{
    public class Client : ClientBase
    {
        public new ClientInfo Info { get => (ClientInfo)base.Info; set => base.Info = value; }
        public Client(ISession session) : base(session)
        {
            Info = new ClientInfo(session.RemoteIPEndPoint);
            Crypto = new GSCrypt();
        }
        protected override void OnConnected()
        {
            base.OnConnected();
            LogWriter.LogNetworkSending(Session.RemoteIPEndPoint, ClientInfo.ChallengeResponse);
            Session.Send(Crypto.Encrypt(UniSpyEncoding.GetBytes(ClientInfo.ChallengeResponse)));
        }
        protected override void OnReceived(object buffer)
        {
            var data = UniSpyEncoding.GetString((byte[])buffer);
            // gamestatus client will send plaintext message 
            // we do not need to decrypt it
            if (data.StartsWith("\u001b\0"))
            {
                base.OnReceived(buffer);
            }
            else
            {
                // we ignore unencrypted message
                return;
            }
        }
        protected override byte[] DecryptMessage(byte[] buffer)
        {
            var data = UniSpyEncoding.GetString(buffer);
            // gamestatus client will send plaintext message 
            // we do not need to decrypt it
            if (data.StartsWith("\u001b\0"))
            {
                return Crypto.Decrypt(buffer);
            }
            else
            {
                return buffer;
            }
        }
    }
}