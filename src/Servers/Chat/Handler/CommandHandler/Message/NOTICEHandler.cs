using Chat.Abstraction.BaseClass;
using Chat.Entity.Structure;
using Chat.Entity.Structure.ChatCommand;
using Chat.Entity.Structure.Response.Message;
using UniSpyLib.Abstraction.Interface;

namespace Chat.Handler.CommandHandler.ChatMessageCommandHandler
{
    public class NOTICEHandler : ChatMessageHandlerBase
    {
        new NOTICERequest _request;
        public NOTICEHandler(IUniSpySession session, ChatRequestBase request) : base(session, request)
        {
            _request = new NOTICERequest(request.RawRequest);
        }
        protected override void CheckRequest()
        {
            base.CheckRequest();
            if (!(bool)_request.Parse())
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
