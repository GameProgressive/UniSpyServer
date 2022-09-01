using UniSpyServer.Servers.Chat.Abstraction.BaseClass;
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
    
    public sealed class GetChannelKeyHandler : ChannelHandlerBase
    {
        private new GetChannelKeyRequest _request => (GetChannelKeyRequest)base._request;
        private new GetChannelKeyResult _result{ get => (GetChannelKeyResult)base._result; set => base._result = value; }
        public GetChannelKeyHandler(IClient client, IRequest request) : base(client, request)
        {
            _result = new GetChannelKeyResult();
        }

        protected override void DataOperation()
        {
            _result.ChannelUserIRCPrefix = _user.Info.IRCPrefix;
            _result.Values = _channel.GetChannelValueString(_request.Keys);
            _result.ChannelName = _channel.Name;
        }

        protected override void ResponseConstruct()
        {
            _response = new GetChannelKeyResponse(_request, _result);
        }
    }
}
