using Chat.Abstraction.BaseClass;
using Chat.Entity.Exception;
using Chat.Entity.Structure.Request;
using Chat.Entity.Structure.Response.Channel;
using Chat.Entity.Structure.Result.Channel;
using UniSpyLib.Abstraction.Interface;

namespace Chat.Handler.CmdHandler.Channel
{
    internal sealed class TOPICHandler : ChatChannelHandlerBase
    {
        private new TOPICRequest _request => (TOPICRequest)base._request;
        private new TOPICResult _result
        {
            get => (TOPICResult)base._result;
            set => base._result = value;
        }
        public TOPICHandler(IUniSpySession session, IUniSpyRequest request) : base(session, request)
        {
            _result = new TOPICResult();
        }

        protected override void DataOperation()
        {
            if (!_user.IsChannelOperator)
            {
                throw new ChatException("Edit topic failed because you are not channel operator.");
            }
            switch (_request.RequestType)
            {
                case TOPICCmdType.GetChannelTopic:
                    break;
                case TOPICCmdType.SetChannelTopic:
                    _channel.Property.ChannelTopic = _request.ChannelTopic;
                    break;
            }
            _result.ChannelName = _channel.Property.ChannelName;
            _result.ChannelTopic = _channel.Property.ChannelTopic;
        }
        protected override void ResponseConstruct()
        {
            _response = new TOPICResponse(_request, _result);
        }
        protected override void Response()
        {
            _response.Build();
            switch (_request.RequestType)
            {
                case TOPICCmdType.GetChannelTopic:
                    _session.SendAsync((string)_response.SendingBuffer);
                    break;
                case TOPICCmdType.SetChannelTopic:
                    _channel.MultiCast((string)_response.SendingBuffer);
                    break;
            }
        }
    }
}
