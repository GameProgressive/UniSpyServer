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
        protected override void OnReceived(object buffer)
        {
            base.OnReceived(buffer);
            var data = Crypto.Decrypt(buffer as byte[]);
            new CmdSwitcher(this, data).Switch();
        }
    }
}