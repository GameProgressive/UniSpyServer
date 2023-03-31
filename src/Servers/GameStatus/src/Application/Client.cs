using UniSpy.Server.GameStatus.Aggregate.Misc;
using UniSpy.Server.GameStatus.Handler;
using UniSpy.Server.Core.Abstraction.BaseClass;
using UniSpy.Server.Core.Abstraction.Interface;
using UniSpy.Server.Core.Encryption;
using UniSpy.Server.Core.Logging;
using System;

namespace UniSpy.Server.GameStatus.Application
{
    public sealed class Client : ClientBase
    {
        public new ClientInfo Info { get => (ClientInfo)base.Info; private set => base.Info = value; }
        public Client(IConnection connection, IServer server) : base(connection, server)
        {
            Info = new ClientInfo();
        }
        protected override void OnConnected()
        {
            Crypto = new GSCrypt();
            this.LogNetworkSending(ClientInfo.ChallengeResponse);
            Connection.Send(Crypto.Encrypt(ClientInfo.ChallengeResponseBytes));
            base.OnConnected();
        }
        protected override byte[] DecryptMessage(byte[] buffer)
        {
            // unit test
            if (Crypto is null)
            {
                return buffer;
            }

            // multiple request;
            var buffers = UniSpyEncoding.GetString(buffer).Split(@"\final\", StringSplitOptions.RemoveEmptyEntries);
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