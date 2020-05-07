using System;
using System.Linq;
using Chat.Entity.Structure;
using Chat.Entity.Structure.ChatCommand;
using Chat.Handler.SystemHandler.ChatSessionManage;
using GameSpyLib.Common.Entity.Interface;

namespace Chat.Handler.CommandHandler
{
    public class USERHandler : ChatCommandHandlerBase
    {
        USER _user;

        public USERHandler(ISession client, ChatCommandBase cmd) : base(client, cmd)
        {
            _user = (USER)cmd;
        }

        public override void DataOperation()
        {
            if (_errorCode != ChatError.NoError)
            {
                return;
            }
            _session.UserInfo.SetUserName(_user.UserName);
            _session.UserInfo.SetName(_user.Name);
            base.DataOperation();
        }

        public override void ConstructResponse()
        {
            if (_errorCode > ChatError.NoError)
            {
                _sendingBuffer =
                    ChatCommandBase.BuildNumericErrorRPL("",
                    _errorCode, $"{_user.NickName} newnick param2", "");
            }
            //_sendingBuffer = new PING().BuildResponse();
            base.ConstructResponse();
        }
    }
}
