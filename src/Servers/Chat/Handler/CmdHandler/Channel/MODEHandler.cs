using Chat.Abstraction.BaseClass;
using Chat.Entity.Structure;
using Chat.Entity.Structure.Misc;
using Chat.Entity.Structure.Misc.ChannelInfo;
using Chat.Entity.Structure.Request;
using Chat.Entity.Structure.Response;
using Chat.Entity.Structure.Result;
using UniSpyLib.Abstraction.Interface;

namespace Chat.Handler.CmdHandler.Channel
{
    internal sealed class MODEHandler : ChatLogedInHandlerBase
    {
        private new MODERequest _request => (MODERequest)base._request;
        private new MODEResult _result
        {
            get => (MODEResult)base._result;
            set => base._result = value;
        }

        ChatChannel _channel;
        ChatChannelUser _user;
        public MODEHandler(IUniSpySession session, IUniSpyRequest request) : base(session, request)
        {
            _result = new MODEResult();
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
                    GetChannelAndUser();
                    break;
            }
        }
        private void GetChannelAndUser()
        {
            if (_session.UserInfo.JoinedChannels.Count == 0)
            {
                _result.ErrorCode = ChatErrorCode.IRCError;
                _result.IRCErrorCode = ChatIRCErrorCode.NoSuchChannel;
                return;
            }

            if (!_session.UserInfo.GetJoinedChannelByName(_request.ChannelName, out _channel))
            {
                _result.ErrorCode = ChatErrorCode.IRCError;
                _result.IRCErrorCode = ChatIRCErrorCode.NoSuchChannel;
                return;
            }

            if (!_channel.GetChannelUserBySession(_session, out _user))
            {
                _result.ErrorCode = ChatErrorCode.IRCError;
                _result.IRCErrorCode = ChatIRCErrorCode.NoSuchNick;
                return;
            }
        }

        protected override void DataOperation()
        {
            switch (_request.RequestType)
            {
                case ModeRequestType.EnableUserQuietFlag:
                    _session.UserInfo.IsQuietMode = true;
                    _result.NickName = _session.UserInfo.NickName;
                    break;
                case ModeRequestType.DisableUserQuietFlag:
                    _session.UserInfo.IsQuietMode = false;
                    _result.NickName = _session.UserInfo.NickName;
                    break;
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
                _result.ErrorCode = ChatErrorCode.DataOperation;
                return;
            }
            _channel.Property.SetProperties(_user, _request);
        }

        protected override void ResponseConstruct()
        {
            _response = new MODEResponse(_request, _result);
        }
    }
}
