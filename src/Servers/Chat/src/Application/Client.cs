using System.Linq;
using UniSpy.Server.Chat.Contract.Request.General;
using UniSpy.Server.Chat.Handler;
using UniSpy.Server.Chat.Handler.CmdHandler.General;
using UniSpy.Server.Core.Abstraction.BaseClass;
using UniSpy.Server.Core.Abstraction.Interface;
using UniSpy.Server.Core.Logging;

namespace UniSpy.Server.Chat.Application
{
    public sealed class Client : ClientBase
    {
        public new ClientInfo Info => (ClientInfo)base.Info;
        public new ITcpConnection Connection => (ITcpConnection)base.Connection;
        private byte[] _incompleteBuffer;
        public Client(IConnection connection) : base(connection)
        {
            base.Info = new ClientInfo();
        }
        protected override void OnReceived(object buffer)
        {
            var message = DecryptMessage((byte[])buffer);
            if (message[message.Length - 1] == 0x0A)
            {
                // check last _incomplteBuffer if it has incomplete message, then combine them
                byte[] completeBuffer;
                if (_incompleteBuffer is not null)
                {
                    completeBuffer = _incompleteBuffer.Concat(message).ToArray();
                    _incompleteBuffer = null;
                }
                else
                {
                    completeBuffer = message;
                }
                this.LogNetworkReceiving((byte[])completeBuffer);
                new CmdSwitcher(this, completeBuffer).Switch();
            }
            else
            {
                // message is not finished, we add it in _completeBuffer
                if (_incompleteBuffer is null)
                {
                    _incompleteBuffer = message;
                }
                else
                {
                    _incompleteBuffer = _incompleteBuffer.Concat(message).ToArray();
                }
            }

        }
        //todo add ondisconnect event process
        protected override void OnDisconnected()
        {
            if (Info.IsLoggedIn)
            {
                var req = new QuitRequest()
                {
                    Reason = $"{Info.NickName} Disconnected."
                };
                new QuitHandler(this, req).Handle();
                Info.IsLoggedIn = false;
            }
            base.OnDisconnected();
        }
        protected override ISwitcher CreateSwitcher(object buffer) => new CmdSwitcher(this, buffer);

    }
}