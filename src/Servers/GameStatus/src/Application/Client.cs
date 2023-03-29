using UniSpy.Server.GameStatus.Aggregate.Misc;
using UniSpy.Server.GameStatus.Handler;
using UniSpy.Server.Core.Abstraction.BaseClass;
using UniSpy.Server.Core.Abstraction.Interface;
using UniSpy.Server.Core.Encryption;
using UniSpy.Server.Core.Logging;
using UniSpy.Server.Core.MiscMethod;

namespace UniSpy.Server.GameStatus.Application
{
    public sealed class Client : ClientBase
    {
        public new ClientInfo Info { get => (ClientInfo)base.Info; private set => base.Info = value; }
        public Client(IConnection connection) : base(connection)
        {
            Info = new ClientInfo();
            Crypto = new GSCrypt();
        }
        protected override void OnConnected()
        {
            base.OnConnected();
            this.LogNetworkSending(ClientInfo.ChallengeResponse);
            Connection.Send(Crypto.Encrypt(ClientInfo.ChallengeResponseBytes));
        }
        protected override byte[] DecryptMessage(byte[] buffer)
        {
            // multiple request;
            var buffers = UniSpyEncoding.GetString(buffer).Split(@"\final\", System.StringSplitOptions.RemoveEmptyEntries);
            if (buffers.Length > 1)
            {
                string message = "";
                foreach (var buf in buffers)
                {
                    var completeBuf = UniSpyEncoding.GetBytes(buf + @"\final\");
                    message += UniSpyEncoding.GetString(Crypto.Decrypt(completeBuf));
                }
                return UniSpyEncoding.GetBytes(message);
            }
            return Crypto.Decrypt(buffer);
        }


        protected override ISwitcher CreateSwitcher(object buffer) => new CmdSwitcher(this, buffer);
    }
}