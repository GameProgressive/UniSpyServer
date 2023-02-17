using UniSpy.Server.GameStatus.Entity.Structure.Misc;
using UniSpy.Server.GameStatus.Handler;
using UniSpy.Server.Core.Abstraction.BaseClass;
using UniSpy.Server.Core.Abstraction.Interface;
using UniSpy.Server.Core.Encryption;
using UniSpy.Server.Core.Logging;

namespace UniSpy.Server.GameStatus.Application
{
    public sealed class Client : ClientBase
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
            this.LogNetworkSending(ClientInfo.ChallengeResponse);
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