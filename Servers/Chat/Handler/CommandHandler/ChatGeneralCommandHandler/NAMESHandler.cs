using Chat.Entity.Structure;
using Chat.Entity.Structure.ChatChannel;
using Chat.Entity.Structure.ChatCommand;
using Chat.Entity.Structure.ChatResponse;
using Chat.Handler.SystemHandler.ChannelManage;
using GameSpyLib.Common.Entity.Interface;

namespace Chat.Handler.CommandHandler.ChatGeneralCommandHandler
{
    public class NAMESHandler : ChatCommandHandlerBase
    {
        new NAMESRequest _request;
        ChatChannelBase _channel;
        ChatChannelUser _user;
        public NAMESHandler(ISession session, ChatRequestBase request) : base(session, request)
        {
            _request = new NAMESRequest(request.RawRequest);
        }

        protected override void CheckRequest()
        {
            base.CheckRequest();
            if (!_request.Parse())
            {
                _errorCode = ChatError.Parse;
                return;
            }

            if (!ChatChannelManager.GetChannel(_request.ChannelName, out _channel))
            {
                _errorCode = ChatError.IRCError;
                _sendingBuffer = ChatIRCError.BuildNoSuchChannelError(_request.ChannelName);
            }

            //can not find any user
            if (!_channel.GetChannelUserBySession(_session, out _user))
            {
                _errorCode = ChatError.IRCError;
                _sendingBuffer = ChatIRCError.BuildNoSuchNickError();
                return;
            }

        }

        protected override void Response()
        {
            _channel.SendChannelUsersToJoiner(_user);
        }
    }
}
