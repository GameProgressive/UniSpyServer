using Chat.Entity.Structure;
using Chat.Entity.Structure.Misc;
using Chat.Entity.Structure.Misc.ChannelInfo;
using UniSpyLib.Abstraction.Interface;

namespace Chat.Abstraction.BaseClass
{
    internal abstract class ChatChannelHandlerBase : ChatLogedInHandlerBase
    {
        protected ChatChannel _channel;
        protected ChatChannelUser _user;
        private new ChatChannelRequestBase _request => (ChatChannelRequestBase)base._request;
        public ChatChannelHandlerBase(IUniSpySession session, IUniSpyRequest request) : base(session, request)
        {
        }

        protected override void RequestCheck()
        {
            if (_session.UserInfo.JoinedChannels.Count == 0)
            {
                _result.ErrorCode = ChatErrorCode.IRCError;
                _result.IRCErrorCode = ChatIRCErrorCode.NoSuchChannel;
                return;
            }

            if (!_session.UserInfo.GetJoinedChannelByName(_request.ChannelName, out _channel))
            {
                _result.ErrorCode = ChatErrorCode.IRCError;
                _result.IRCErrorCode = ChatIRCErrorCode.NoSuchChannel;
                return;
            }

            if (!_channel.GetChannelUserBySession(_session, out _user))
            {
                _result.ErrorCode = ChatErrorCode.IRCError;
                _result.IRCErrorCode = ChatIRCErrorCode.NoSuchNick;
                return;
            }
        }
    }
}
