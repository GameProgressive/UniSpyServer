using Chat.Entity.Structure.ChatCommand;
using Chat.Entity.Structure.ChatResponse;
using GameSpyLib.Common.Entity.Interface;

namespace Chat.Handler.CommandHandler
{
    public class NOTICEHandler : ChatJoinedChannelHandlerBase
    {
        new NOTICE _cmd;
        public NOTICEHandler(ISession session, ChatCommandBase cmd) : base(session, cmd)
        {
            _cmd = (NOTICE)cmd;
        }

        public override void DataOperation()
        {
            base.DataOperation();
            _sendingBuffer = 
            ChatReply.BuildNoticeReply(_user, _cmd.ChannelName, _cmd.Message);
        }

        public override void Response()
        {
            if (_sendingBuffer == null || _sendingBuffer == "" || _sendingBuffer.Length < 3)
            {
                return;
            }
            _channel.MultiCast(_sendingBuffer);
        }
    }
}
