using UniSpyServer.Servers.Chat.Abstraction.BaseClass;
using UniSpyServer.Servers.Chat.Entity.Exception.IRC.Channel;
using UniSpyServer.Servers.Chat.Entity.Exception.IRC.General;
using UniSpyServer.Servers.Chat.Entity.Structure.Request.Channel;
using UniSpyServer.Servers.Chat.Entity.Structure.Response.Channel;
using UniSpyServer.Servers.Chat.Entity.Structure.Result.Channel;
using UniSpyServer.UniSpyLib.Abstraction.Interface;

namespace UniSpyServer.Servers.Chat.Handler.CmdHandler.Channel
{
    /// <summary>
    /// Get or set channel or user mode
    /// </summary>
    
    public sealed class ModeHandler : ChannelHandlerBase
    {
        private new ModeRequest _request => (ModeRequest)base._request;
        private new ModeResult _result { get => (ModeResult)base._result; set => base._result = value; }
        public ModeHandler(IClient client, IRequest request) : base(client, request) { }
        protected override void RequestCheck()
        {
            if (_request.RawRequest is null)
            {
                _channel = _client.Info.GetJoinedChannel(_request.ChannelName);
                if (_channel is null)
                {
                    throw new ChatIRCNoSuchChannelException($"No such channel {_request.ChannelName}", _request.ChannelName);
                }
                _user = _channel.GetChannelUser(_client);
                if (_user is null)
                {
                    throw new ChatIRCNoSuchNickException($"Can not find user with nickname: {_client.Info.NickName} username: {_client.Info.UserName}");
                }
                return;
            }
            base.RequestCheck();
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
