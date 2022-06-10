using UniSpyServer.Servers.Chat.Abstraction.BaseClass;

using UniSpyServer.Servers.Chat.Entity.Exception;
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
    
    public sealed class SetChannelKeyHandler : ChannelHandlerBase
    {
        private new SetChannelKeyRequest _request => (SetChannelKeyRequest)base._request;
        private new SetChannelKeyResult _result{ get => (SetChannelKeyResult)base._result; set => base._result = value; }
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
            _channel.SetChannelKeyValue(_request.KeyValue);
            _result.ChannelName = _result.ChannelName;
            _result.ChannelUserIRCPrefix = _user.Info.IRCPrefix;

        }
        protected override void ResponseConstruct()
        {
            _response = new SetChannelKeyResponse(_request, _result);
        }
    }
}
