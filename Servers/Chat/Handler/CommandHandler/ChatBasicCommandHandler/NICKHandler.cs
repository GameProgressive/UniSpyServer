using Chat.Entity.Structure;
using Chat.Entity.Structure.ChatCommand;
using Chat.Entity.Structure.ChatResponse;
using Chat.Handler.SystemHandler.ChatSessionManage;
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
                _errorCode = ChatError.IRCError;
                _sendingBuffer = ChatIRCError.BuildNoSuchNickError();
            }
            base.CheckRequest();
        }

        public override void DataOperation()
        {
            base.DataOperation();
            _session.UserInfo.SetNickName(_nickCmd.NickName);
        }

        public override void ConstructResponse()
        {
            base.ConstructResponse();

            _sendingBuffer =
                ChatReply.BuildWelcomeReply(_session.UserInfo);

            _session.UserInfo.SetLoginFlag(true);

           
            //check this!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!
            //_sendingBuffer += ChatCommandBase.BuildMessageRPL(
            //_nickCmd.NickName, $"MODE {_nickCmd.NickName}", "+iwx");
        }
    }
}
