using UniSpyServer.Servers.Chat.Abstraction.BaseClass;
using UniSpyServer.Servers.Chat.Entity.Exception;
using UniSpyServer.Servers.Chat.Entity.Structure.Request.Channel;
using UniSpyServer.Servers.Chat.Entity.Structure.Response.Channel;
using UniSpyServer.Servers.Chat.Entity.Structure.Result.Channel;
using UniSpyServer.UniSpyLib.Abstraction.Interface;

namespace UniSpyServer.Servers.Chat.Handler.CmdHandler.Channel
{
    
    public sealed class TopicHandler : ChannelHandlerBase
    {
        private new TopicRequest _request => (TopicRequest)base._request;
        private new TopicResult _result { get => (TopicResult)base._result; set => base._result = value; }
        public TopicHandler(IClient client, IRequest request) : base(client, request)
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
                    _channel.Topic = _request.ChannelTopic;
                    break;
            }
            _result.ChannelName = _channel.Name;
            _result.ChannelTopic = _channel.Topic;
        }
        protected override void ResponseConstruct()
        {
            _response = new TopicResponse(_request, _result);
        }
        protected override void Response()
        {
            switch (_request.RequestType)
            {
                case TopicRequestType.GetChannelTopic:
                    _client.Send(_response);
                    break;
                case TopicRequestType.SetChannelTopic:
                    _channel.MultiCast(_response);
                    break;
            }
        }
    }
}
