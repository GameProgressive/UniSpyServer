using UniSpy.Server.Chat.Abstraction.BaseClass;
using UniSpy.Server.Chat.Exception;
using UniSpy.Server.Chat.Contract.Request.Channel;
using UniSpy.Server.Chat.Contract.Response.Channel;
using UniSpy.Server.Chat.Contract.Result.Channel;
using UniSpy.Server.Core.Abstraction.Interface;

namespace UniSpy.Server.Chat.Handler.CmdHandler.Channel
{
    // Sets channel key/values.
    // If user is NULL or "", the keys will be set on the channel.
    // Otherwise, they will be set on the user,
    // Only ops can set channel keys on other users.
    // Set a value to NULL or "" to clear that key.
    public sealed class SetChannelKeyHandler : ChannelHandlerBase
    {
        private new SetChannelKeyRequest _request => (SetChannelKeyRequest)base._request;
        private new SetChannelKeyResult _result { get => (SetChannelKeyResult)base._result; set => base._result = value; }
        public SetChannelKeyHandler(IClient client, IRequest request) : base(client, request)
        {
            _result = new SetChannelKeyResult();
        }

        protected override void DataOperation()
        {
            if (!_user.IsChannelOperator)
            {
                throw new ChatException("SETCHANKEY failed because you are not channel operator.");
            }
            _channel.KeyValues.Update(_request.KeyValues);

            _result.ChannelName = _result.ChannelName;
            _result.ChannelUserIRCPrefix = _user.Info.IRCPrefix;
        }
        protected override void ResponseConstruct()
        {
            _response = new SetChannelKeyResponse(_request, _result);
        }
        //! setchankey must be broadcast to every one in this channel except sender
        protected override void Response()
        {
            _channel.MultiCast(_client, _response, true);
        }
    }
}
