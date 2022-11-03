using System.Linq;
using UniSpyServer.Servers.Chat.Entity.Structure.Request.General;
using UniSpyServer.Servers.Chat.Handler;
using UniSpyServer.Servers.Chat.Handler.CmdHandler.General;
using UniSpyServer.UniSpyLib.Abstraction.BaseClass;
using UniSpyServer.UniSpyLib.Abstraction.Interface;
using UniSpyServer.UniSpyLib.Logging;

namespace UniSpyServer.Servers.Chat.Entity.Structure
{
    public class Client : ClientBase
    {
        public new ClientInfo Info => (ClientInfo)base.Info;
        public new ITcpSession Session => (ITcpSession)base.Session;
        private byte[] _incompleteBuffer;
        public Client(ISession session) : base(session)
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
                LogWriter.LogNetworkReceiving(Session.RemoteIPEndPoint, (byte[])completeBuffer);
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