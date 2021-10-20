using UniSpyServer.Chat.Abstraction.BaseClass;
using UniSpyServer.Chat.Entity.Contract;
using UniSpyServer.Chat.Entity.Structure.Request;
using UniSpyServer.Chat.Entity.Structure.Response;
using UniSpyServer.Chat.Entity.Structure.Result.Channel;
using UniSpyServer.UniSpyLib.Abstraction.Interface;

namespace UniSpyServer.Chat.Handler.CmdHandler.Channel
{
    /// <summary>
    /// Get or set channel or user mode
    /// </summary>
    [HandlerContract("MODE")]
    public sealed class ModeHandler : ChannelHandlerBase
    {
        private new ModeRequest _request => (ModeRequest)base._request;
        private new ModeResult _result
        {
            get => (ModeResult)base._result;
            set => base._result = value;
        }
        public ModeHandler(IUniSpySession session, IUniSpyRequest request) : base(session, request)
        {
            _result = new ModeResult();
        }

        protected override void RequestCheck()
        {
            base.RequestCheck();
            switch (_request.RequestType)
            {
                case ModeRequestType.EnableUserQuietFlag:
                case ModeRequestType.DisableUserQuietFlag:
                    //we do not need to find user and its channel here
                    break;
                default:
                    base.RequestCheck();
                    break;
            }
        }

        protected override void DataOperation()
        {
            switch (_request.RequestType)
            {
                case ModeRequestType.EnableUserQuietFlag:
                    _session.UserInfo.IsQuietMode = true;
                    _result.JoinerNickName = _session.UserInfo.NickName;
                    break;
                case ModeRequestType.DisableUserQuietFlag:
                    _session.UserInfo.IsQuietMode = false;
                    _result.JoinerNickName = _session.UserInfo.NickName;
                    break;
                // We get user nick name then get channel modes
                case ModeRequestType.GetChannelUserModes:
                    _result.JoinerNickName = _request.NickName;
                    goto case ModeRequestType.GetChannelModes;
                case ModeRequestType.GetChannelModes:
                    _result.ChannelModes = _channel.Property.ChannelMode.GetChannelMode();
                    _result.ChannelName = _channel.Property.ChannelName;
                    break;
                default:
                    ProcessOtherModeRequest();
                    break;
            }
        }

        private void ProcessOtherModeRequest()
        {
            //we check if the user is operator in channel
            if (!_user.IsChannelOperator)
            {
                return;
            }
            _channel.Property.SetProperties(_user, _request);
        }

        protected override void ResponseConstruct()
        {
            _response = new ModeResponse(_request, _result);
        }
    }
}
