using Chat.Entity.Structure.ChatCommand;
using Chat.Entity.Structure.ChatResponse;
using GameSpyLib.Common.Entity.Interface;

namespace Chat.Handler.CommandHandler
{
    public class NOTICEHandler : ChatMessageHandlerBase
    {
        new NOTICE _cmd;
        public NOTICEHandler(ISession session, ChatCommandBase cmd) : base(session, cmd)
        {
            _cmd = (NOTICE)cmd;
        }

        public override void DataOperation()
        {
            base.DataOperation();
            switch (_cmd.RequestType)
            {
                case ChatMessageType.ChannelMessage:
                    _sendingBuffer = ChatReply.BuildNoticeReply(
                            _user.UserInfo, _cmd.NickName, _cmd.Message);
                    break;
                case ChatMessageType.UserMessage:
                    _sendingBuffer = ChatReply.BuildNoticeReply(
                        _user.UserInfo, _cmd.NickName, _cmd.Message);
                    break;
            }
        }
    }
}
