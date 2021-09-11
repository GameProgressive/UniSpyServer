using Chat.Abstraction.BaseClass;
using Chat.Entity.Contract;
using Chat.Entity.Exception;
using Chat.Entity.Exception.IRC.General;
using Chat.Entity.Structure.Misc.ChannelInfo;
using Chat.Entity.Structure.Request;
using Chat.Entity.Structure.Response.Channel;
using Chat.Entity.Structure.Result.Channel;
using UniSpyLib.Abstraction.Interface;

namespace Chat.Handler.CmdHandler.Channel
{
    // Sets channel key/values.
    // If user is NULL or "", the keys will be set on the channel.
    // Otherwise, they will be set on the user,
    // Only ops can set channel keys on other users.
    // Set a value to NULL or "" to clear that key.
    [HandlerContract("SETCKEY")]
    internal sealed class SetCKeyHandler : ChannelHandlerBase
    {
        private new SetCKeyRequest _request => (SetCKeyRequest)base._request;
        private new SetCKeyResult _result
        {
            get => (SetCKeyResult)base._result;
            set => base._result = value;
        }
        ChannelUser _otherUser;
        public SetCKeyHandler(IUniSpySession session, IUniSpyRequest request) : base(session, request)
        {
        }

        protected override void RequestCheck()
        {
            base.RequestCheck();
            if (_request.NickName != _session.UserInfo.NickName)
            {
                if (!_user.IsChannelOperator)
                {
                    throw new Exception("SETCKEY failed because you are not channel operator.");
                }
                _result.IsSetOthersKeyValue = true;
                _otherUser = _channel.GetChannelUserByNickName(_request.NickName);
                if (_otherUser == null)
                {
                    throw new ChatIRCNoSuchNickException($"Can not find user:{_request.NickName} in channel {_request.ChannelName}");

                }
            }
        }

        protected override void DataOperation()
        {
            _result.ChannelName = _channel.Property.ChannelName;
            if (_result.IsSetOthersKeyValue)
            {
                _otherUser.UpdateUserKeyValues(_request.KeyValues);
                _result.NickName = _otherUser.UserInfo.NickName;
            }
            else
            {
                _user.UpdateUserKeyValues(_request.KeyValues);
                _result.NickName = _user.UserInfo.NickName;
            }
        }

        protected override void ResponseConstruct()
        {
            _response = new SetCKeyResponse(_request, _result);
        }

        protected override void Response()
        {
            _response.Build();
            _channel.MultiCast((string)_response.SendingBuffer);
        }
    }
}
