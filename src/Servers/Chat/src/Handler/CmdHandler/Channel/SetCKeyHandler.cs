using UniSpy.Server.Chat.Abstraction.BaseClass;
using UniSpy.Server.Chat.Error.IRC.General;
using UniSpy.Server.Chat.Aggregate;
using UniSpy.Server.Chat.Contract.Request.Channel;
using UniSpy.Server.Chat.Contract.Response.Channel;
using UniSpy.Server.Chat.Contract.Result.Channel;
using UniSpy.Server.Core.Abstraction.Interface;
using UniSpy.Server.Chat.Abstraction.Interface;

namespace UniSpy.Server.Chat.Handler.CmdHandler.Channel
{
    // Sets channel key/values.
    // If user is NULL or "", the keys will be set on the channel.
    // Otherwise, they will be set on the user,
    // Only ops can set channel keys on other users.
    // Set a value to NULL or "" to clear that key.

    public sealed class SetCKeyHandler : ChannelHandlerBase
    {
        private new SetCKeyRequest _request => (SetCKeyRequest)base._request;
        private new SetCKeyResult _result { get => (SetCKeyResult)base._result; set => base._result = value; }
        private ChannelUser _otherUser;
        public SetCKeyHandler(IChatClient client, SetCKeyRequest request) : base(client, request)
        {
            _result = new SetCKeyResult();
        }

        protected override void RequestCheck()
        {
            base.RequestCheck();
            if (_request.NickName != _client.Info.NickName)
            {
                if (!_user.IsChannelOperator)
                {
                    throw new Chat.Exception("SETCKEY failed because you are not channel operator.");
                }
                _result.IsSetOthersKeyValue = true;
                _otherUser = _channel.GetUser(_request.NickName);
                if (_otherUser is null)
                {
                    throw new NoSuchNickException($"Can not find user:{_request.NickName} in channel {_request.ChannelName}");
                }
            }
        }

        protected override void DataOperation()
        {
            _result.ChannelName = _channel.Name;
            if (_result.IsSetOthersKeyValue)
            {
                _otherUser.KeyValues.Update(_request.KeyValues);
                _result.NickName = _otherUser.Info.NickName;
            }
            else
            {
                _user.KeyValues.Update(_request.KeyValues);
                _result.NickName = _user.Info.NickName;
            }
        }

        protected override void ResponseConstruct()
        {
            if (_request.IsBroadCast)
            {
                _response = new SetCKeyResponse(_request, _result);
            }
        }
        //! if there are key start with b_ we must broadcast to everyone
        protected override void Response()
        {
            _channel.MultiCast(_client, _response);
        }

    }
}
