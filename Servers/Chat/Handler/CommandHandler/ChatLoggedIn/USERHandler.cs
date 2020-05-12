using Chat.Entity.Structure;
using Chat.Entity.Structure.ChatCommand;
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
    }
}
