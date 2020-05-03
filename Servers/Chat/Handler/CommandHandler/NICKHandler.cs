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
        public NICKHandler(ISession client, ChatCommandBase cmd) : base(client, cmd)
        {
            _nickCmd = (NICK)cmd;
        }

        public override void CheckRequest()
        {
            if (ChatSessionManager.IsNickNameExisted(_nickCmd.NickName))
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
            _session.UserInfo.NickName = _nickCmd.NickName;
        }

        public override void ConstructResponse()
        {
            base.ConstructResponse();

            if (_errorCode > ChatError.NoError)
            {
                _sendingBuffer = ChatCommandBase.BuildNumericErrorRPL(
                    _errorCode,$"* {_nickCmd.NickName}","Nick name in use!");
                return;
            }
            _sendingBuffer = ChatCommandBase.BuildNumericRPL(
                ChatServer.ServerDomain, ChatResponseType.Welcome,
                _nickCmd.NickName, "Welcome to RetroSpy!");
            //check this!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!
            //_sendingBuffer += ChatCommandBase.BuildMessageRPL(
            //_nickCmd.NickName, $"MODE {_nickCmd.NickName}", "+iwx");
        }
    }
}
