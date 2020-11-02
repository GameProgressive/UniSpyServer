using Chat.Abstraction.BaseClass;
using Chat.Entity.Structure.ChatCommand;
using Chat.Entity.Structure.ChatResponse.ChatMessageResponse;
using UniSpyLib.Abstraction.Interface;

namespace Chat.Handler.CommandHandler.ChatMessageCommandHandler
{
    public class ATMHandler : ChatMessageHandlerBase
    {
        new ATMRequest _request;
        public ATMHandler(ISession session, ChatRequestBase request) : base(session, request)
        {
            _request = (ATMRequest)request;
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
