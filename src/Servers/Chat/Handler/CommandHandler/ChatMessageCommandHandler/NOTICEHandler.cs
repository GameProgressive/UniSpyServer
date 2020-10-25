using Chat.Entity.Structure;
using Chat.Entity.Structure.ChatCommand;
using Chat.Entity.Structure.ChatResponse.ChatMessageResponse;
using GameSpyLib.Common.Entity.Interface;

namespace Chat.Handler.CommandHandler.ChatMessageCommandHandler
{
    public class NOTICEHandler : ChatMessageHandlerBase
    {
        new NOTICERequest _request;
        public NOTICEHandler(ISession session, ChatRequestBase request) : base(session, request)
        {
            _request = new NOTICERequest(request.RawRequest);
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
                    _sendingBuffer = NOTICEReply.BuildNoticeReply(
                            _user.UserInfo, _request.ChannelName, _request.Message);
                    break;
                case ChatMessageType.UserMessage:
                    _sendingBuffer = NOTICEReply.BuildNoticeReply(
                        _session.UserInfo, _request.NickName, _request.Message);
                    break;
            }
        }
    }
}
