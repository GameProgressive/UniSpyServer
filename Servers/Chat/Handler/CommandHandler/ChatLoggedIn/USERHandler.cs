using Chat.Entity.Structure;
using Chat.Entity.Structure.ChatCommand;
using Chat.Entity.Structure.ChatResponse;
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
            base.DataOperation();
            _session.UserInfo.SetUserName(_user.UserName);
            _session.UserInfo.SetName(_user.Name);
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
