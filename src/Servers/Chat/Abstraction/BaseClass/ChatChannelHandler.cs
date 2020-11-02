using Chat.Entity.Structure;
using Chat.Entity.Structure.ChatChannel;
using Chat.Entity.Structure.ChatResponse;
using UniSpyLib.Abstraction.Interface;

namespace Chat.Abstraction.BaseClass
{
    public class ChatChannelHandlerBase : ChatLogedInHandlerBase
    {
        protected ChatChannelBase _channel;
        protected ChatChannelUser _user;
        protected new ChatChannelRequestBase _request;

        public ChatChannelHandlerBase(ISession session, ChatRequestBase request) : base(session, request)
        {
            _request = (ChatChannelRequestBase)request;
        }

        protected override void CheckRequest()
        {
            base.CheckRequest();

            if (_session.UserInfo.JoinedChannels.Count == 0)
            {
                _errorCode = ChatError.IRCError;
                _sendingBuffer = ChatIRCError.BuildNoSuchChannelError(_request.ChannelName);
                return;
            }

            if (!_session.UserInfo.GetJoinedChannelByName(_request.ChannelName, out _channel))
            {
                _errorCode = ChatError.IRCError;
                _sendingBuffer = ChatIRCError.BuildNoSuchChannelError(_request.ChannelName);
                return;
            }

            if (!_channel.GetChannelUserBySession(_session, out _user))
            {
                _errorCode = ChatError.Parse;
                _sendingBuffer = ChatIRCError.BuildNoSuchNickError();
                return;
            }

        }
    }
}
