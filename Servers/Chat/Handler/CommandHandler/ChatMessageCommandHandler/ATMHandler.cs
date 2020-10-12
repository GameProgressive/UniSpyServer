using Chat.Entity.Structure;
using Chat.Entity.Structure.ChatCommand;
using Chat.Entity.Structure.ChatResponse.ChatMessageResponse;
using GameSpyLib.Common.Entity.Interface;

namespace Chat.Handler.CommandHandler.ChatMessageCommandHandler
{
    public class ATMHandler : ChatMessageHandlerBase
    {
        new ATMRequest _request;

        public ATMHandler(ISession session, ChatRequestBase request) : base(session, request)
        {
            _request = new ATMRequest(request.RawRequest);
        }
        protected override void CheckRequest()
        {
            base.CheckRequest();
            if (!_request.Parse())
            {
                _errorCode = ChatError.Parse;
                return;
            }
        }
        protected override void BuildNormalResponse()
        {
            base.BuildNormalResponse();
            switch (_request.RequestType)
            {
                case ChatMessageType.ChannelMessage:
                    _sendingBuffer =
                        ATMReply.BuildATMReply(
                        _user.UserInfo, _request.ChannelName, _request.Message);
                    break;
                case ChatMessageType.UserMessage:
                    _sendingBuffer =
                        ATMReply.BuildATMReply(
                        _session.UserInfo, _request.NickName, _request.Message);
                    break;
            }
        }
    }
}
