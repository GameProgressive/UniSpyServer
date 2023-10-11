using UniSpy.Server.Chat.Abstraction.BaseClass;
using UniSpy.Server.Chat.Abstraction.Interface;
using UniSpy.Server.Chat.Contract.Request.Channel;
using UniSpy.Server.Chat.Contract.Response.Channel;
using UniSpy.Server.Chat.Contract.Result.Channel;

namespace UniSpy.Server.Chat.Handler.CmdHandler.Channel
{
    /// <summary>
    /// Get or set channel or user mode
    /// </summary>

    public sealed class ModeHandler : ChannelHandlerBase
    {
        private new ModeRequest _request => (ModeRequest)base._request;
        private new ModeResult _result { get => (ModeResult)base._result; set => base._result = value; }
        public ModeHandler(IShareClient client, ModeRequest request) : base(client, request) { }
        public ModeHandler(IShareClient client, ModeRequest request, Aggregate.Channel channel, Aggregate.ChannelUser user) : base(client, request)
        {
            _user = user;
            _channel = channel;
        }
        protected override void DataOperation()
        {
            _result = new ModeResult();
            switch (_request.RequestType)
            {
                // We get user nick name then get channel modes
                case ModeRequestType.GetChannelAndUserModes:
                    _result.JoinerNickName = _request.NickName;
                    goto case ModeRequestType.GetChannelModes;
                case ModeRequestType.GetChannelModes:
                    _result.ChannelModes = _channel.Mode.ToString();
                    _result.ChannelName = _channel.Name;
                    break;
                case ModeRequestType.SetChannelModes:
                    _channel.SetProperties(_user, _request);
                    break;
            }
        }
        protected override void PublishMessage()
        {
            if (_request.RequestType == ModeRequestType.SetChannelModes)
            {
                base.PublishMessage();
            }
        }
        protected override void ResponseConstruct()
        {
            // we only response to get channel modes
            switch (_request.RequestType)
            {
                case ModeRequestType.GetChannelAndUserModes:
                case ModeRequestType.GetChannelModes:
                    _response = new ModeResponse(_request, _result);
                    break;
                case ModeRequestType.SetChannelModes:
                    break;
            }
        }
    }
}
