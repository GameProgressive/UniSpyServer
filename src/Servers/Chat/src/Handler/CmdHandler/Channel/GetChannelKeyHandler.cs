using UniSpy.Server.Chat.Abstraction.BaseClass;
using UniSpy.Server.Chat.Abstraction.Interface;
using UniSpy.Server.Chat.Contract.Request.Channel;
using UniSpy.Server.Chat.Contract.Response.Channel;
using UniSpy.Server.Chat.Contract.Result.Channel;

namespace UniSpy.Server.Chat.Handler.CmdHandler.Channel
{
    // Sets channel key/values.
    // If user is NULL or "", the keys will be set on the channel.
    // Otherwise, they will be set on the user,
    // Only ops can set channel keys on other users.
    // Set a value to NULL or "" to clear that key.

    public sealed class GetChannelKeyHandler : ChannelHandlerBase
    {
        private new GetChannelKeyRequest _request => (GetChannelKeyRequest)base._request;
        private new GetChannelKeyResult _result { get => (GetChannelKeyResult)base._result; set => base._result = value; }
        public GetChannelKeyHandler(IShareClient client, GetChannelKeyRequest request) : base(client, request)
        {
            _result = new GetChannelKeyResult();
        }

        protected override void DataOperation()
        {
            _result.ChannelUserIRCPrefix = _user.Info.IRCPrefix;
            _result.Values = _channel.KeyValues.GetValueString(_request.Keys);
            _result.ChannelName = _channel.Name;
        }

        protected override void ResponseConstruct()
        {
            _response = new GetChannelKeyResponse(_request, _result);
        }
    }
}
