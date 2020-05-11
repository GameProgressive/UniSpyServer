using System;
using Chat.Entity.Structure.ChatChannel;
using Chat.Entity.Structure.ChatCommand;
using Chat.Entity.Structure.ChatResponse;
using GameSpyLib.Common.Entity.Interface;

namespace Chat.Handler.CommandHandler
{
    public class ATMHandler : ChatCommandHandlerBase
    {
        new ATM _cmd;
        ChatChannelBase _channel;
        ChatChannelUser _user;
        public ATMHandler(ISession session, ChatCommandBase cmd) : base(session, cmd)
        {
            _cmd = (ATM)cmd;
        }

        public override void CheckRequest()
        {
            base.CheckRequest();
            if (!_session.UserInfo.GetJoinedChannelByName(_cmd.ChannelName, out _channel))
            {
                _errorCode = Entity.Structure.ChatError.Parse;
                return;
            }

            if (!_channel.GetChannelUserBySession(_session, out _user))
            {
                _errorCode = Entity.Structure.ChatError.Parse;
                return;
            }

        }

        public override void DataOperation()
        {
            base.DataOperation();
            switch (_cmd.RequestType)
            {
                case ATMCmdType.ChannelATM:
                    _sendingBuffer = _user.BuildReply(ChatReply.ATM,$"{_channel.Property.ChannelName} {_cmd.Message}");
                    //ChatCommandBase.BuildMessageRPL(
                    //    $"ATM {_channel.Property.ChannelName} ", _cmd.Message);
                    break;
                case ATMCmdType.UserATM:
                    _sendingBuffer = _user.BuildReply(ChatReply.ATM,$"{_cmd.NickName} {_cmd.Message}");
                    //ChatCommandBase.BuildMessageRPL(
                    //    $"ATM {_cmd.NickName}", _cmd.Message);
                    break;
            }
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
