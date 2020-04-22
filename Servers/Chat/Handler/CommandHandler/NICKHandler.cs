using System;
using System.Linq;
using Chat.Entity.Structure;
using Chat.Entity.Structure.ChatCommand;
using Chat.Handler.SystemHandler.ChatSessionManage;
using Chat.Server;
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

        public override void CheckRequest()
        {
            if (ChatSessionManager.Sessions.
                Where(s => s.Value.ClientInfo.NickName == _nickCmd.NickName).Count() != 0)
            {
                _errorCode = ChatError.NickNameInUse;
            }
            base.CheckRequest();
        }

        public override void DataOperation()
        {
            base.DataOperation();
            if (_errorCode > ChatError.NoError)
            {
                return;
            }
            _session.ClientInfo.NickName = _nickCmd.NickName;
        }

        public override void ConstructResponse()
        {
            base.ConstructResponse();

            if (_errorCode > ChatError.NoError)
            {
                _sendingBuffer = ChatCommandBase.BuildErrorRPL(
                    _errorCode,$"* {_nickCmd.NickName}","Nick name in use!");
                return;
            }
            _sendingBuffer = ChatCommandBase.BuildNormalRPL(
                ChatServer.ServerDomain, ChatResponseType.Welcome,
                _nickCmd.NickName, "Welcome to RetroSpy!");
            //check this!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!
            //_sendingBuffer += ChatCommandBase.BuildMessageRPL(_nickCmd.NickName, $"MODE {_nickCmd.NickName}", "+iwx");
        }
    }
}
