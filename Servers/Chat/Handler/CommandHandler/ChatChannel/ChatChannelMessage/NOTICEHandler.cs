using Chat.Entity.Structure.ChatCommand;
using Chat.Entity.Structure.ChatResponse;
using GameSpyLib.Common.Entity.Interface;

namespace Chat.Handler.CommandHandler
{
    public class NOTICEHandler : ChatMessageHandlerBase
    {
        new NoticeRequest _request;
        public NOTICEHandler(ISession session, ChatRequestBase cmd) : base(session, cmd)
        {
            _request = (NoticeRequest)cmd;
        }

        protected override void DataOperation()
        {
            base.DataOperation();
            switch (_request.RequestType)
            {
                case ChatMessageType.ChannelMessage:
                    _sendingBuffer = ChatReply.BuildNoticeReply(
                            _user.UserInfo, _request.ChannelName, _request.Message);
                    break;
                case ChatMessageType.UserMessage:
                    _sendingBuffer = ChatReply.BuildNoticeReply(
                        _session.UserInfo, _request.NickName, _request.Message);
                    break;
            }
        }
    }
}
