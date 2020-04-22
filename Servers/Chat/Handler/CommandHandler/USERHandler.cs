using System;
using System.Linq;
using Chat.Entity.Structure;
using Chat.Entity.Structure.ChatCommand;
using Chat.Handler.SystemHandler.ChatSessionManage;
using GameSpyLib.Common.Entity.Interface;

namespace Chat.Handler.CommandHandler
{
    public class USERHandler: ChatCommandHandlerBase
    {
        USER _user;

        public USERHandler(IClient client, ChatCommandBase cmd) : base(client, cmd)
        {
            _user = (USER)cmd;
        }

        public override void DataOperation()
        {
            if (_errorCode != ChatError.NoError)
            {
                return;
            }
            _session.ClientInfo.UserName = _user.UserName;
            _session.ClientInfo.Name = _user.Name;
            base.DataOperation();
        }

        public override void ConstructResponse()
        {
            if (_errorCode > ChatError.NoError)
            {
                _sendingBuffer =
                    ChatCommandBase.BuildErrorRPL("",
                    _errorCode, $"{_user.NickName} newnick param2", "");
            }
            //_sendingBuffer = new PING().BuildResponse();
            base.ConstructResponse();
        }
    }
}
