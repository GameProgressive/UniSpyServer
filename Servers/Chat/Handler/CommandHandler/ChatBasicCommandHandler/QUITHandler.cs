using Chat.Entity.Structure.ChatChannel;
using Chat.Entity.Structure.ChatCommand;
using Chat.Handler.SystemHandler.ChatSessionManage;
using GameSpyLib.Common.Entity.Interface;

namespace Chat.Handler.CommandHandler
{
    public class QUITHandler : ChatCommandHandlerBase
    {
        new QUIT _cmd;
        ChatChannelUser _user;
        public QUITHandler(ISession client, ChatCommandBase cmd) : base(client, cmd)
        {
            _cmd = (QUIT)cmd;
        }

        public override void DataOperation()
        {
            base.DataOperation();



            foreach (var channel in _session.UserInfo.JoinedChannels)
            {
                if (!channel.GetChannelUserBySession(_session,out _user))
                {
                    continue;
                }

                channel.RemoveBindOnUserAndChannel(_user);
                channel.MultiCastLeave(_user, _cmd.Reason);
            }
            ChatSessionManager.RemoveSession(_session);
        }
    }
}
