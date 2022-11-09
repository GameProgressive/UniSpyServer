using UniSpyServer.Servers.GameStatus.Entity.Structure.Misc;
using UniSpyServer.Servers.GameStatus.Handler;
using UniSpyServer.UniSpyLib.Abstraction.BaseClass;
using UniSpyServer.UniSpyLib.Abstraction.Interface;
using UniSpyServer.UniSpyLib.Encryption;
using UniSpyServer.UniSpyLib.Logging;

namespace UniSpyServer.Servers.GameStatus.Entity.Structure
{
    public class Client : ClientBase
    {
        public new ClientInfo Info { get => (ClientInfo)base.Info; set => base.Info = value; }
        public Client(IConnection connection) : base(connection)
        {
            Info = new ClientInfo();
            Crypto = new GSCrypt();
        }
        protected override void OnConnected()
        {
            base.OnConnected();
            LogWriter.LogNetworkSending(Connection.RemoteIPEndPoint, ClientInfo.ChallengeResponse);
            Connection.Send(Crypto.Encrypt(UniSpyEncoding.GetBytes(ClientInfo.ChallengeResponse)));
        }
        protected override void OnReceived(object buffer)
        {
            var data = UniSpyEncoding.GetString((byte[])buffer);
            // gamestatus client will send plaintext message 
            // we do not need to decrypt it
            // if (data.StartsWith("\u001b\0"))
            // if (data[0] == '\u001b' && data[1] == '\0')
            // {
            base.OnReceived(buffer);
            // }
            // else
            // {
            //     // we ignore unencrypted message
            //     return;
            // }
        }
        protected override byte[] DecryptMessage(byte[] buffer)
        {
            var data = UniSpyEncoding.GetString(buffer);
            // gamestatus client will send plaintext message 
            // we do not need to decrypt it
            // if (data.StartsWith("\u001b\0"))
            if (data[0] == '\u001b' && data[1] == '\0')
            {
                return Crypto.Decrypt(buffer);
            }
            else
            {
                return buffer;
            }
        }
        protected override ISwitcher CreateSwitcher(object buffer) => new CmdSwitcher(this, buffer);
    }
}