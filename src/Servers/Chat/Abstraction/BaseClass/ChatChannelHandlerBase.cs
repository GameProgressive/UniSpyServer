using Chat.Entity.Structure;
using Chat.Entity.Structure.ChannelInfo;
using Chat.Entity.Structure.ChannelInfo;
using Chat.Entity.Structure.Response;
using UniSpyLib.Abstraction.Interface;

namespace Chat.Abstraction.BaseClass
{
    public class ChatChannelHandlerBase : ChatLogedInHandlerBase
    {
        protected ChatChannel _channel;
        protected ChatChannelUser _user;
        protected new ChatChannelRequestBase _request
        {
            get { return (ChatChannelRequestBase)base._request; }
        }

        public ChatChannelHandlerBase(IUniSpySession session, IUniSpyRequest request) : base(session, request)
        {
        }

        protected override void CheckRequest()
        {
            base.CheckRequest();

            if (_session.UserInfo.JoinedChannels.Count == 0)
            {
                _errorCode = ChatErrorCode.IRCError;
                _sendingBuffer = ChatIRCError.BuildNoSuchChannelError(_request.ChannelName);
                return;
            }

            if (!_session.UserInfo.GetJoinedChannelByName(_request.ChannelName, out _channel))
            {
                _errorCode = ChatErrorCode.IRCError;
                _sendingBuffer = ChatIRCError.BuildNoSuchChannelError(_request.ChannelName);
                return;
            }

            if (!_channel.GetChannelUserBySession(_session, out _user))
            {
                _errorCode = ChatErrorCode.Parse;
                _sendingBuffer = ChatIRCError.BuildNoSuchNickError();
                return;
            }

        }
    }
}
