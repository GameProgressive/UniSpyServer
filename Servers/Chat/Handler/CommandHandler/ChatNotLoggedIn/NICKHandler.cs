using Chat.Entity.Structure;
using Chat.Entity.Structure.ChatCommand;
using Chat.Entity.Structure.ChatResponse;
using Chat.Handler.SystemHandler.ChatSessionManage;
using GameSpyLib.Common.Entity.Interface;

namespace Chat.Handler.CommandHandler
{
    public class NICKHandler : ChatCommandHandlerBase
    {
        new NICK _cmd;
        public NICKHandler(ISession client, ChatCommandBase cmd) : base(client, cmd)
        {
            _cmd = (NICK)cmd;
        }

        public override void CheckRequest()
        {

            base.CheckRequest();

            if (ChatSessionManager.IsNickNameExisted(_cmd.NickName))
            {
                _errorCode = ChatError.IRCError;
                _sendingBuffer = ChatIRCError.BuildNoSuchNickError();
            }

        }

        public override void DataOperation()
        {
            base.DataOperation();
            _session.UserInfo.SetNickName(_cmd.NickName);
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
