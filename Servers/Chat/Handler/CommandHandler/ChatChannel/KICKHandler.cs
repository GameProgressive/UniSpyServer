using Chat.Entity.Structure;
using Chat.Entity.Structure.ChatChannel;
using Chat.Entity.Structure.ChatCommand;
using Chat.Entity.Structure.ChatResponse;
using GameSpyLib.Common.Entity.Interface;

namespace Chat.Handler.CommandHandler
{
    public class KICKHandler : ChatChannelHandlerBase
    {
        new KICK _cmd;
        ChatChannelUser _kickee;
        public KICKHandler(ISession session, ChatCommandBase cmd) : base(session, cmd)
        {
            _cmd = (KICK)cmd;
        }

        public override void CheckRequest()
        {
            base.CheckRequest();
            if (_errorCode != ChatError.NoError)
            {
                return;
            }

            if (!_user.IsChannelOperator)
            {
                _errorCode = ChatError.NotChannelOperator;
                return;
            }
            if (!_channel.GetChannelUserByNickName(_cmd.NickName, out _kickee))
            {
                _errorCode = ChatError.Parse;
                return;
            }
        }


        public override void DataOperation()
        {
            base.DataOperation();
            _sendingBuffer =
                ChatReply.BuildKickReply(
                    _channel.Property.ChannelName,
                    _user, _kickee, _cmd.Reason);
        }

        public override void Response()
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
