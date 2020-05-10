using Chat.Entity.Structure.ChatChannel;
using Chat.Entity.Structure.ChatCommand;
using Chat.Handler.SystemHandler.ChannelManage;
using GameSpyLib.Common.Entity.Interface;

namespace Chat.Handler.CommandHandler
{
    public class NAMESHandler : ChatCommandHandlerBase
    {
        NAMES _namesCmd;
        ChatChannelBase _channel;
        public NAMESHandler(ISession client, ChatCommandBase cmd) : base(client, cmd)
        {
            _namesCmd = (NAMES)cmd;
        }

        public override void CheckRequest()
        {
            base.CheckRequest();
        }

        public override void ConstructResponse()
        {
            ChatChannelUser user;

            //can not find any user
            if (!_channel.GetChannelUserBySession(_session, out user))
            {
                _systemError = Entity.Structure.ChatError.DataOperation;
                return;
            }

            _channel.SendChannelUsersToJoiner(user);

            base.ConstructResponse();
        }

        public override void DataOperation()
        {
            ChatChannelManager.Channels.TryGetValue(_namesCmd.ChannelName, out _channel);
            if (_channel == null)
            {
                _systemError = Entity.Structure.ChatError.DataOperation;
            }
            base.DataOperation();
        }
    }
}
