using Chat.Entity.Structure;
using Chat.Entity.Structure.ChatCommand;
using Chat.Entity.Structure.ChatResponse;
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
                _systemError = ChatError.IRCError;
                _ircErrorCode = IRCError.NickNameInUse;
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

            _sendingBuffer = _session.UserInfo.BuildReply(ChatReply.Welcome, _nickCmd.NickName, "Welcome to RetrosSpy!");
                //ChatCommandBase.BuildRPLWithoutMiddle(
                //    ChatRPL.Welcome, _nickCmd.NickName,
                //    "Welcome to RetroSpy!");

               // ChatCommandBase.BuildNumericRPL(
               // ChatServer.ServerDomain, ChatResponseType.Welcome,
               //_nickCmd.NickName, "Welcome to RetroSpy!");

            _session.UserInfo.SetLoginFlag(true);

           
            //check this!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!
            //_sendingBuffer += ChatCommandBase.BuildMessageRPL(
            //_nickCmd.NickName, $"MODE {_nickCmd.NickName}", "+iwx");
        }
    }
}
