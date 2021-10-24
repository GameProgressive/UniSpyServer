using UniSpyServer.Chat.Abstraction.BaseClass;
using UniSpyServer.Chat.Entity.Contract;
using UniSpyServer.Chat.Entity.Exception;
using UniSpyServer.Chat.Entity.Structure.Request.Channel;
using UniSpyServer.Chat.Entity.Structure.Response.Channel;
using UniSpyServer.Chat.Entity.Structure.Result.Channel;
using UniSpyServer.UniSpyLib.Abstraction.Interface;

namespace UniSpyServer.Chat.Handler.CmdHandler.Channel
{
    // Sets channel key/values.
    // If user is NULL or "", the keys will be set on the channel.
    // Otherwise, they will be set on the user,
    // Only ops can set channel keys on other users.
    // Set a value to NULL or "" to clear that key.
    [HandlerContract("SETCHANKEY")]
    public sealed class SetChannelKeyHandler : ChannelHandlerBase
    {
        private new SetChannelKeyRequest _request => (SetChannelKeyRequest)base._request;
        private new SetChannelKeyResult _result
        {
            get => (SetChannelKeyResult)base._result;
            set => base._result = value;
        }
        public SetChannelKeyHandler(IUniSpySession session, IUniSpyRequest request) : base(session, request)
        {
            _result = new SetChannelKeyResult();
        }

        protected override void DataOperation()
        {
            if (!_user.IsChannelOperator)
            {
                throw new Exception("SETCHANKEY failed because you are not channel operator.");
            }
            _channel.Property.SetChannelKeyValue(_request.KeyValue);
            _result.ChannelName = _result.ChannelName;
            _result.ChannelUserIRCPrefix = _user.UserInfo.IRCPrefix;

        }
        protected override void ResponseConstruct()
        {
            _response = new SetChannelKeyResponse(_request, _result);
        }
    }
}
