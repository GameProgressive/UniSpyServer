using Chat.Entity.Structure.ChatChannel;
using Chat.Entity.Structure.ChatCommand;
using Chat.Entity.Structure.ChatResponse;
using GameSpyLib.Common.Entity.Interface;

namespace Chat.Handler.CommandHandler.ChatChannelCommandHandler
{
    public class TOPICHandler : ChatCommandHandlerBase
    {
        new TOPICRequest _request;
        ChatChannelBase _channel;
        ChatChannelUser _user;
        public TOPICHandler(ISession session, ChatRequestBase request) : base(session, request)
        {
            _request = (TOPICRequest)request;
        }

        protected override void CheckRequest()
        {
            base.CheckRequest();
            if (!_session.UserInfo.GetJoinedChannelByName(_request.ChannelName, out _channel))
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

        protected override void DataOperation()
        {
            base.DataOperation();
            if (!_user.IsChannelOperator)
            {
                return;
            }
            switch (_request.RequestType)
            {
                case TOPICCmdType.GetChannelTopic:
                    GetChannelTopic();
                    break;
                case TOPICCmdType.SetChannelTopic:
                    SetChannelTopic();
                    break;
            }

        }

        protected override void ConstructResponse()
        {
            base.ConstructResponse();
            if (_errorCode > Entity.Structure.ChatError.NoError)
            {
                //we handle error code response here
            }
        }
        private void GetChannelTopic()
        {
            if (_channel.Property.ChannelTopic == "" || _channel.Property.ChannelTopic == null)
            {
                _sendingBuffer =
                    ChatReply.BuildNoTopicReply(
                    _channel.Property.ChannelName);
            }
            else
            {
                _sendingBuffer =
                    ChatReply.BuildTopicReply(
                    _channel.Property.ChannelName,
                    _channel.Property.ChannelTopic);
            }
        }
        
        private void SetChannelTopic()
        {
            _channel.Property.SetChannelTopic(_request.ChannelTopic);
            _sendingBuffer =
                ChatReply.BuildTopicReply(
                    _channel.Property.ChannelName,
                    _channel.Property.ChannelTopic);
        }

        protected override void Response()
        {
            base.Response();
            if (_sendingBuffer == null || _sendingBuffer == "" || _sendingBuffer.Length < 3)
            {
                return;
            }
            switch (_request.RequestType)
            {
                case TOPICCmdType.GetChannelTopic:
                    _session.SendAsync(_sendingBuffer);
                    break;
                case TOPICCmdType.SetChannelTopic:
                    _channel.MultiCast(_sendingBuffer);
                    break;
            }

        }
    }
}
