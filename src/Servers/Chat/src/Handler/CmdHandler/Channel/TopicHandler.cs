using UniSpyServer.Chat.Abstraction.BaseClass;
using UniSpyServer.Chat.Entity.Contract;
using UniSpyServer.Chat.Entity.Exception;
using UniSpyServer.Chat.Entity.Structure.Request;
using UniSpyServer.Chat.Entity.Structure.Response.Channel;
using UniSpyServer.Chat.Entity.Structure.Result.Channel;
using UniSpyServer.UniSpyLib.Abstraction.Interface;

namespace UniSpyServer.Chat.Handler.CmdHandler.Channel
{
    [HandlerContract("TOPIC")]
    public sealed class TopicHandler : ChannelHandlerBase
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
                throw new Exception("Edit topic failed because you are not channel operator.");
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
