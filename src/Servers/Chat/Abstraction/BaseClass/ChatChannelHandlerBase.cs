using Chat.Entity.Structure;
using Chat.Entity.Structure.Misc;
using Chat.Entity.Structure.Misc.ChannelInfo;
using UniSpyLib.Abstraction.Interface;

namespace Chat.Abstraction.BaseClass
{
    public abstract class ChatChannelHandlerBase : ChatLogedInHandlerBase
    {
        protected ChatChannel _channel;
        protected ChatChannelUser _user;
        protected new ChatChannelRequestBase _request
        => (ChatChannelRequestBase)base._request;
        protected new ChatResultBase _result
        {
            get => (ChatResultBase)base._result;
            set => base._result = value;
        }

        public ChatChannelHandlerBase(IUniSpySession session, IUniSpyRequest request) : base(session, request)
        {
        }

        protected override void RequestCheck()
        {
            if (_session.UserInfo.JoinedChannels.Count == 0)
            {
                _errorCode = ChatErrorCode.NoSuchChannel;
                _result.ErrorCode = ChatIRCErrorCode.NoSuchChannel;
                _sendingBuffer = ChatIRCErrorCode.BuildNoSuchChannelError(_request.ChannelName);
                return;
            }

            if (!_session.UserInfo.GetJoinedChannelByName(_request.ChannelName, out _channel))
            {
                _errorCode = ChatErrorCode.IRCError;
                _sendingBuffer = ChatIRCErrorCode.BuildNoSuchChannelError(_request.ChannelName);
                return;
            }

            if (!_channel.GetChannelUserBySession(_session, out _user))
            {
                _errorCode = ChatErrorCode.Parse;
                _sendingBuffer = ChatIRCErrorCode.BuildNoSuchNickError();
                return;
            }

        }
    }
}
