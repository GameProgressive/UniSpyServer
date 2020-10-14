using Chat.Entity.Structure;
using Chat.Entity.Structure.ChatChannel;
using Chat.Entity.Structure.ChatCommand;
using Chat.Entity.Structure.ChatResponse.ChatChannelResponse;
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
                _errorCode = ChatError.Parse;
                return;
            }
            if (!_channel.GetChannelUserBySession(_session, out _user))
            {
                _errorCode = ChatError.Parse;
                return;
            }
        }

        protected override void DataOperation()
        {
            base.DataOperation();
            if (!_user.IsChannelOperator)
            {
                _errorCode = ChatError.NotChannelOperator;
                return;
            }
        }

        protected override void BuildNormalResponse()
        {
            base.BuildNormalResponse();
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

        private void GetChannelTopic()
        {
            if (_channel.Property.ChannelTopic == "" || _channel.Property.ChannelTopic == null)
            {
                _sendingBuffer =
                    TOPICReply.BuildNoTopicReply(
                    _channel.Property.ChannelName);
            }
            else
            {
                _sendingBuffer =
                    TOPICReply.BuildTopicReply(
                    _channel.Property.ChannelName,
                    _channel.Property.ChannelTopic);
            }
        }

        private void SetChannelTopic()
        {
            _channel.Property.SetChannelTopic(_request.ChannelTopic);
            _sendingBuffer =
                TOPICReply.BuildTopicReply(
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
