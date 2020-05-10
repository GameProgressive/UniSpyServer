using System;
using Chat.Entity.Structure.ChatChannel;
using Chat.Entity.Structure.ChatCommand;
using Chat.Entity.Structure.ChatResponse;
using GameSpyLib.Common.Entity.Interface;

namespace Chat.Handler.CommandHandler
{
    public class NOTICEHandler : ChatCommandHandlerBase
    {
        new NOTICE _cmd;
        ChatChannelBase _channel;
        ChatChannelUser _user;
        public NOTICEHandler(ISession session, ChatCommandBase cmd) : base(session, cmd)
        {
            _cmd = (NOTICE)cmd;
        }

        public override void CheckRequest()
        {
            base.CheckRequest();
            if (!_session.UserInfo.GetJoinedChannel(_cmd.ChannelName, out _channel))
            {
                _systemError = Entity.Structure.ChatError.Parse;
                return;
            }

            if (!_channel.GetChannelUserBySession(_session, out _user))
            {
                _systemError = Entity.Structure.ChatError.Parse;
                return;
            }
        }

        public override void DataOperation()
        {
            base.DataOperation();
            _sendingBuffer = 
                ChatCommandBase.BuildReply(ChatReply.NOTICE,
                    $"{_cmd.ChannelName}", _cmd.Message);
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
