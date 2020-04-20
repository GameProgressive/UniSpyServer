using Chat.Entity.Structure;
using Chat.Entity.Structure.ChatCommand;
using Chat.Entity.Structure.Enumerator.Request;
using GameSpyLib.Common.Entity.Interface;
using System.Net;

namespace Chat.Handler.CommandHandler
{
    public class USRIPHandler : ChatCommandHandlerBase
    {
        USRIP _usripCmd { get { return (USRIP)_cmd; } }

        public USRIPHandler(IClient client, ChatCommandBase cmd) : base(client, cmd)
        {

        }

        public override void ConstructResponse()
        {
            base.ConstructResponse();

            string ip = ((IPEndPoint)_session.Socket.RemoteEndPoint).Address.ToString();

            _sendingBuffer = _usripCmd.BuildResponse(ip);
        }
    }
}
