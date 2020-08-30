using Chat.Entity.Structure;
using Chat.Entity.Structure.ChatChannel;
using Chat.Entity.Structure.ChatCommand;
using Chat.Entity.Structure.ChatResponse;
using GameSpyLib.Common.Entity.Interface;

namespace Chat.Handler.CommandHandler.ChatChannelCommandHandler
{
    public class KICKHandler : ChatChannelHandlerBase
    {
        new KICKRequest _request;
        ChatChannelUser _kickee;
        public KICKHandler(ISession session, ChatRequestBase request) : base(session, request)
        {
            _request = new KICKRequest(request.RawRequest);
        }

        protected override void CheckRequest()
        {
            base.CheckRequest();

            if (!_request.Parse())
            {
                _errorCode = ChatError.Parse;
                return;
            }
            if (!_user.IsChannelOperator)
            {
                _errorCode = ChatError.NotChannelOperator;
                return;
            }
            if (!_channel.GetChannelUserByNickName(_request.NickName, out _kickee))
            {
                _errorCode = ChatError.Parse;
                return;
            }
        }


        protected override void DataOperation()
        {
            base.DataOperation();
            _sendingBuffer =
                ChatReply.BuildKickReply(
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
