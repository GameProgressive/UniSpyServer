using Chat.Entity.Structure;
using Chat.Entity.Structure.ChatChannel;
using Chat.Entity.Structure.ChatCommand;
using Chat.Entity.Structure.ChatResponse;
using Chat.Handler.SystemHandler.ChannelManage;
using GameSpyLib.Common.Entity.Interface;

namespace Chat.Handler.CommandHandler
{
    public class NAMESHandler : ChatCommandHandlerBase
    {
        new NAMES _cmd;
        ChatChannelBase _channel;
        ChatChannelUser _user;
        public NAMESHandler(ISession client, ChatCommandBase cmd) : base(client, cmd)
        {
            _cmd = (NAMES)cmd;
        }

        public override void CheckRequest()
        {
            base.CheckRequest();
            //can not find any user
            if (!_channel.GetChannelUserBySession(_session, out _user))
            {
                _errorCode = ChatError.IRCError;
                _sendingBuffer = ChatIRCError.BuildNoSuchNickError();
                return;
            }
            if (!ChatChannelManager.GetChannel(_cmd.ChannelName, out _channel))
            {
                _errorCode = ChatError.IRCError;
                _sendingBuffer = ChatIRCError.BuildNoSuchChannelError(_cmd.ChannelName);
            }
        }

        public override void Response()
        {
            _channel.SendChannelUsersToJoiner(_user);
        }
    }
}
