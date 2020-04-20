using System;
using Chat.Entity.Structure;
using Chat.Entity.Structure.ChatCommand;
using GameSpyLib.Common.Entity.Interface;

namespace Chat.Handler.CommandHandler
{
    public class NICKHandler : ChatCommandHandlerBase
    {
        NICK _nickCmd;
        public NICKHandler(IClient client, ChatCommandBase cmd) : base(client, cmd)
        {
            _nickCmd = (NICK)cmd;
        }

        public override void DataOperation()
        {
            base.DataOperation();
            _session.ClientInfo.NickName = _nickCmd.NickName;
        }

        public override void ConstructResponse()
        {
            base.ConstructResponse();
            _sendingBuffer = _nickCmd.BuildResponse(_session.ClientInfo.NickName);
        }
    }
}
