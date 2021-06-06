using Chat.Entity.Exception;
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
            _channel = _session.UserInfo.GetJoinedChannelByName(_request.ChannelName);
            _user = _channel.GetChannelUserBySession(_session);
            if (_user == null)
            {
                throw new ChatIRCException($"Can not find user with nickname: {_session.UserInfo.NickName} username: {_session.UserInfo.UserName}", ChatIRCErrorCode.NoSuchNick);
            }
        }
    }
}
