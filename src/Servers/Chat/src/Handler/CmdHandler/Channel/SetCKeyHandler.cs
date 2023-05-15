using UniSpy.Server.Chat.Abstraction.BaseClass;
using UniSpy.Server.Chat.Error.IRC.General;
using UniSpy.Server.Chat.Aggregate;
using UniSpy.Server.Chat.Contract.Request.Channel;
using UniSpy.Server.Chat.Contract.Response.Channel;
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
        private ChannelUser _otherUser;
        public SetCKeyHandler(IChatClient client, SetCKeyRequest request) : base(client, request)
        {
        }

        protected override void RequestCheck()
        {
            _request.Parse();
            // if client nickname is *, we set the name and execute the stored handlers
            if (!_client.Info.IsNickNameSet)
            {
                _client.Info.NickName = _request.NickName;
                lock (_client.Info.HandlerStack)
                {
                    foreach (var handler in _client.Info.HandlerStack)
                    {
                        handler.Handle();
                    }
                    _client.Info.HandlerStack.Clear();
                }
            }

            base.RequestCheck();
            if (_request.NickName != _client.Info.NickName)
            {
                if (!_user.IsChannelOperator)
                {
                    throw new Chat.Exception("SETCKEY failed because you are not channel operator.");
                }
                _otherUser = _channel.GetUser(_request.NickName);
                if (_otherUser is null)
                {
                    throw new NoSuchNickException($"Can not find user:{_request.NickName} in channel {_request.ChannelName}");
                }
            }
        }

        protected override void DataOperation()
        {
            if (_otherUser is not null)
            {
                _otherUser.KeyValues.Update(_request.KeyValues);
            }
            else
            {
                _user.KeyValues.Update(_request.KeyValues);
            }
        }

        protected override void ResponseConstruct()
        {
            if (_request.IsBroadCast)
            {
                _response = new SetCKeyResponse(_request);
            }
        }
        //! if there are key start with b_ we must broadcast to everyone
        protected override void Response()
        {
            _channel.MultiCast(_client, _response);
        }
    }
}
