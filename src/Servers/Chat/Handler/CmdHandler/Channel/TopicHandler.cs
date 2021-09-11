using Chat.Abstraction.BaseClass;
using Chat.Entity.Contract;
using Chat.Entity.Exception;
using Chat.Entity.Structure.Request;
using Chat.Entity.Structure.Response.Channel;
using Chat.Entity.Structure.Result.Channel;
using UniSpyLib.Abstraction.Interface;

namespace Chat.Handler.CmdHandler.Channel
{
    [HandlerContract("TOPIC")]
    internal sealed class TopicHandler : ChannelHandlerBase
    {
        private new TopicRequest _request => (TopicRequest)base._request;
        private new TopicResult _result
        {
            get => (TopicResult)base._result;
            set => base._result = value;
        }
        public TopicHandler(IUniSpySession session, IUniSpyRequest request) : base(session, request)
        {
            _result = new TopicResult();
        }

        protected override void DataOperation()
        {
            if (!_user.IsChannelOperator)
            {
                throw new ChatException("Edit topic failed because you are not channel operator.");
            }
            switch (_request.RequestType)
            {
                case TopicRequestType.GetChannelTopic:
                    break;
                case TopicRequestType.SetChannelTopic:
                    _channel.Property.ChannelTopic = _request.ChannelTopic;
                    break;
            }
            _result.ChannelName = _channel.Property.ChannelName;
            _result.ChannelTopic = _channel.Property.ChannelTopic;
        }
        protected override void ResponseConstruct()
        {
            _response = new TopicResponse(_request, _result);
        }
        protected override void Response()
        {
            _response.Build();
            switch (_request.RequestType)
            {
                case TopicRequestType.GetChannelTopic:
                    _session.SendAsync((string)_response.SendingBuffer);
                    break;
                case TopicRequestType.SetChannelTopic:
                    _channel.MultiCast((string)_response.SendingBuffer);
                    break;
            }
        }
    }
}
