using Chat.Abstraction.BaseClass;
using Chat.Entity.Structure;
using Chat.Entity.Structure.ChatCommand;
using Chat.Entity.Structure.Misc.ChannelInfo;
using Chat.Entity.Structure.Response.Channel;
using UniSpyLib.Abstraction.Interface;

namespace Chat.Handler.CmdHandler.Channel
{
    public class KICKHandler : ChatChannelHandlerBase
    {
        new KICKRequest _request { get { return (KICKRequest)base._request; } }
        ChatChannelUser _kickee;
        public KICKHandler(IUniSpySession session, IUniSpyRequest request) : base(session, request)
        {
        }

        protected override void RequestCheck()
        {
            base.RequestCheck();

            if (!_user.IsChannelOperator)
            {
                _errorCode = ChatErrorCode.NotChannelOperator;
                return;
            }
            if (!_channel.GetChannelUserByNickName(_request.NickName, out _kickee))
            {
                _errorCode = ChatErrorCode.Parse;
                return;
            }
        }

        protected override void BuildNormalResponse()
        {
            base.BuildNormalResponse();
            _sendingBuffer = KICKReply.BuildKickReply(
            _channel.Property.ChannelName,
            _user, _kickee, _request.Reason);
        }

        protected override void Response()
        {
            if (_sendingBuffer == null || _sendingBuffer == "" || _sendingBuffer.Length < 3)
            {
                return;
            }
            _channel.MultiCast(_sendingBuffer);
            _channel.RemoveBindOnUserAndChannel(_kickee);
        }
    }
}
