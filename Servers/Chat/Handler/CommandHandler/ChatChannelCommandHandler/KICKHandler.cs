using Chat.Entity.Structure;
using Chat.Entity.Structure.ChatChannel;
using Chat.Entity.Structure.ChatCommand;
using Chat.Entity.Structure.ChatResponse;
using GameSpyLib.Common.Entity.Interface;

namespace Chat.Handler.CommandHandler
{
    public class KICKHandler : ChatCommandHandlerBase
    {
        new KICK _cmd;
        ChatChannelBase _channel;
        ChatChannelUser _kicker;
        ChatChannelUser _kickee;
        public KICKHandler(ISession session, ChatCommandBase cmd) : base(session, cmd)
        {
            _cmd = (KICK)cmd;
        }

        public override void CheckRequest()
        {
            base.CheckRequest();
            if (!_session.UserInfo.GetJoinedChannelByName(_cmd.ChannelName, out _channel))
            {
                _errorCode = ChatError.Parse;
                return;
            }

            if (!_channel.GetChannelUserBySession(_session, out _kicker))
            {
                _errorCode = ChatError.Parse;
                return;
            }
            if (!_kicker.IsChannelOperator)
            {

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
            _sendingBuffer = _kicker.BuildReply(ChatReply.KICK,
                $"KICK {_channel.Property.ChannelName} {_kicker.UserInfo.NickName} {_kickee.UserInfo.NickName}",
                _cmd.Reason);
        }

        public override void ConstructResponse()
        {
            base.ConstructResponse();
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
