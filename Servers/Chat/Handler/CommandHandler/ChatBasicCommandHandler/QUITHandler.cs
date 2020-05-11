using Chat.Entity.Structure.ChatCommand;
using Chat.Handler.SystemHandler.ChatSessionManage;
using GameSpyLib.Common.Entity.Interface;

namespace Chat.Handler.CommandHandler
{
    public class QUITHandler : ChatLogedInHandlerBase
    {
        new QUIT _cmd;
        public QUITHandler(ISession client, ChatCommandBase cmd) : base(client, cmd)
        {
            _cmd = (QUIT)cmd;
        }

        public override void DataOperation()
        {
            base.DataOperation();

            foreach (var channel in _session.UserInfo.JoinedChannels)
            {
                channel.LeaveChannel(_session,_cmd.Reason);
            }
            ChatSessionManager.RemoveSession(_session);
        }
    }
}
