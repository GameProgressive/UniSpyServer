using System;
using Chat.Entity.Structure;
using Chat.Entity.Structure.ChatCommand;
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

        public override void CheckRequest()
        {
            base.CheckRequest();
        }

        public override void DataOperation()
        {
            _session.ClientInfo.UserName = _user.UserName;
            _session.ClientInfo.Name = _user.Name;
            base.DataOperation();
        }

        //public override void ConstructResponse()
        //{
        //    _sendingBuffer = new PING().BuildResponse();
        //    base.ConstructResponse();
        //}
    }
}
