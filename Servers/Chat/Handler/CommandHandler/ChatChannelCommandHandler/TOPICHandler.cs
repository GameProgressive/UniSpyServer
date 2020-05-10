using Chat.Entity.Structure.ChatChannel;
using Chat.Entity.Structure.ChatCommand;
using Chat.Entity.Structure.ChatResponse;
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
            if (!_user.IsChannelOperator)
            {
                return;
            }
            switch (_cmd.RequestType)
            {
                case TOPICCmdType.GetChannelTopic:
                    GetChannelTopic();
                    break;
                case TOPICCmdType.SetChannelTopic:
                    SetChannelTopic();
                    break;
            }

        }

        public override void ConstructResponse()
        {
            base.ConstructResponse();
            if (_systemError > Entity.Structure.ChatError.NoError)
            {
                //we handle error code response here
            }
        }
        private void GetChannelTopic()
        {
            if (_channel.Property.ChannelTopic == "" || _channel.Property.ChannelTopic == null)
            {
                _sendingBuffer =
                    ChatCommandBase.BuildReply(
                        ChatReply.NoTopic,
                        _channel.Property.ChannelName);
            }
            else
            {
                _sendingBuffer =
                        ChatCommandBase.BuildReply(
                            ChatReply.TOPIC,
                        _channel.Property.ChannelName,
                        _channel.Property.ChannelTopic);
            }
        }
        
        private void SetChannelTopic()
        {
            _channel.Property.SetChannelTopic(_cmd.ChannelTopic);
            _sendingBuffer =
                ChatCommandBase.BuildReply(ChatReply.Topic,
                _channel.Property.ChannelName,
                _channel.Property.ChannelTopic);
        }
    }
}
