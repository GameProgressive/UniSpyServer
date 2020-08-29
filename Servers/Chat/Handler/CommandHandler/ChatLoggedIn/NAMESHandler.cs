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
        new NAMES _request;
        ChatChannelBase _channel;
        ChatChannelUser _user;
        public NAMESHandler(ISession session, ChatRequestBase request) : base(session, request)
        {
            _request = (NAMES)request;
        }

        protected override void CheckRequest()
        {
            base.CheckRequest();
            //can not find any user
            if (!_channel.GetChannelUserBySession(_session, out _user))
            {
                _errorCode = ChatError.IRCError;
                _sendingBuffer = ChatIRCError.BuildNoSuchNickError();
                return;
            }
            if (!ChatChannelManager.GetChannel(_request.ChannelName, out _channel))
            {
                _errorCode = ChatError.IRCError;
                _sendingBuffer = ChatIRCError.BuildNoSuchChannelError(_request.ChannelName);
            }
        }

        protected override void Response()
        {
            _channel.SendChannelUsersToJoiner(_user);
        }
    }
}
