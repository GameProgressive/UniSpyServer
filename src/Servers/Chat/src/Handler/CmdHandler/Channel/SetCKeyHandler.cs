using UniSpyServer.Servers.Chat.Abstraction.BaseClass;
using UniSpyServer.Servers.Chat.Entity.Contract;
using UniSpyServer.Servers.Chat.Entity.Exception;
using UniSpyServer.Servers.Chat.Entity.Exception.IRC.General;
using UniSpyServer.Servers.Chat.Entity.Structure.Misc.ChannelInfo;
using UniSpyServer.Servers.Chat.Entity.Structure.Request.Channel;
using UniSpyServer.Servers.Chat.Entity.Structure.Response.Channel;
using UniSpyServer.Servers.Chat.Entity.Structure.Result.Channel;
using UniSpyServer.UniSpyLib.Abstraction.Interface;

namespace UniSpyServer.Servers.Chat.Handler.CmdHandler.Channel
{
    // Sets channel key/values.
    // If user is NULL or "", the keys will be set on the channel.
    // Otherwise, they will be set on the user,
    // Only ops can set channel keys on other users.
    // Set a value to NULL or "" to clear that key.
    [HandlerContract("SETCKEY")]
    public sealed class SetCKeyHandler : ChannelHandlerBase
    {
        private new SetCKeyRequest _request => (SetCKeyRequest)base._request;
        private new SetCKeyResult _result { get => (SetCKeyResult)base._result; set => base._result = value; }
        private ChannelUser _otherUser;
        public SetCKeyHandler(IClient client, IRequest request) : base(client, request)
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
            _result.ChannelName = _channel.Name;
            if (_result.IsSetOthersKeyValue)
            {
                _otherUser.UpdateUserKeyValues(_request.KeyValues);
                _result.NickName = _otherUser.Info.NickName;
            }
            else
            {
                _user.UpdateUserKeyValues(_request.KeyValues);
                _result.NickName = _user.Info.NickName;
            }
        }

        protected override void ResponseConstruct()
        {
            _response = new SetCKeyResponse(_request, _result);
        }

        protected override void Response()
        {
            _channel.MultiCast(_response);
        }
    }
}
