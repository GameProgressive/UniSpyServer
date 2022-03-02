using UniSpyServer.Servers.Chat.Abstraction.BaseClass;
using UniSpyServer.Servers.Chat.Entity.Contract;
using UniSpyServer.Servers.Chat.Entity.Structure.Request.Channel;
using UniSpyServer.Servers.Chat.Entity.Structure.Response.Channel;
using UniSpyServer.Servers.Chat.Entity.Structure.Result.Channel;
using UniSpyServer.UniSpyLib.Abstraction.Interface;

namespace UniSpyServer.Servers.Chat.Handler.CmdHandler.Channel
{
    /// <summary>
    /// Get or set channel or user mode
    /// </summary>
    [HandlerContract("MODE")]
    public sealed class ModeHandler : ChannelHandlerBase
    {
        private new ModeRequest _request => (ModeRequest)base._request;
        private new ModeResult _result { get => (ModeResult)base._result; set => base._result = value; }
        public ModeHandler(IClient client, IRequest request) : base(client, request)
        {
        }

        protected override void DataOperation()
        {
            _result = new ModeResult();
            switch (_request.RequestType)
            {
                // We get user nick name then get channel modes
                case ModeRequestType.GetChannelUserModes:
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

        protected override void ResponseConstruct()
        {
            // we only response to get channel modes
            switch (_request.RequestType)
            {
                case ModeRequestType.GetChannelUserModes:
                case ModeRequestType.GetChannelModes:
                    _response = new ModeResponse(_request, _result);
                    break;
                case ModeRequestType.SetChannelModes:
                    break;
            }
        }
    }
}
