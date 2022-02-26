using UniSpyServer.Servers.GameStatus.Entity.Structure.Misc;
using UniSpyServer.Servers.GameStatus.Handler;
using UniSpyServer.UniSpyLib.Abstraction.BaseClass;
using UniSpyServer.UniSpyLib.Abstraction.Interface;
using UniSpyServer.UniSpyLib.Encryption;

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
            Session.Send(Crypto.Encrypt(UniSpyEncoding.GetBytes(ClientInfo.ChallengeResponse)));
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