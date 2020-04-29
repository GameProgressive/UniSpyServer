using Chat.Entity.Structure.ChatChannel;
using Chat.Entity.Structure.ChatCommand;
using GameSpyLib.Common.Entity.Interface;

namespace Chat.Handler.CommandHandler
{
    public class TOPICHandler : ChatCommandHandlerBase
    {
        new TOPIC _cmd;
        ChatChannelBase _channel;
        ChatChannelUser _user;
        public TOPICHandler(ISession client, ChatCommandBase cmd) : base(client, cmd)
        {
            _cmd = (TOPIC)cmd;
        }

        public override void CheckRequest()
        {
            base.CheckRequest();
            if (!_session.UserInfo.GetJoinedChannel(_cmd.ChannelName, out _channel))
            {
                _errorCode = Entity.Structure.ChatError.Parse;
                return;
            }
            if (!_channel.GetChannelUser(_session, out _user))
            {
                _errorCode = Entity.Structure.ChatError.Parse;
                return;
            }
        }

        public override void DataOperation()
        {
            base.DataOperation();
            if (!_user.IsChannelOperator)
            {
                return;
            }
            _channel.Property.SetChannelTopic(_cmd.ChannelTopic);
        }

        public override void ConstructResponse()
        {
            base.ConstructResponse();
            if (_errorCode > Entity.Structure.ChatError.NoError)
            {

            }
            else
            {
                _sendingBuffer =
                    ChatCommandBase.BuildMessageRPL(
                        $"TOPIC {_channel.Property.ChannelName}",
                        _channel.Property.ChannelTopic);
            }
        }

    }
}
