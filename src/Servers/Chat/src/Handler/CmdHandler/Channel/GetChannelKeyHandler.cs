using Chat.Abstraction.BaseClass;
using Chat.Entity.Contract;
using Chat.Entity.Structure.Request.Channel;
using Chat.Entity.Structure.Response.Channel;
using Chat.Entity.Structure.Result;
using UniSpyLib.Abstraction.Interface;

namespace Chat.Handler.CmdHandler.Channel
{
    // Sets channel key/values.
    // If user is NULL or "", the keys will be set on the channel.
    // Otherwise, they will be set on the user,
    // Only ops can set channel keys on other users.
    // Set a value to NULL or "" to clear that key.
    [HandlerContract("GETCHANKEY")]
    public sealed class GetChannelKeyHandler : ChannelHandlerBase
    {
        private new GetChannelKeyRequest _request => (GetChannelKeyRequest)base._request;
        private new GetChannelKeyResult _result
        {
            get => (GetChannelKeyResult)base._result;
            set => base._result = value;
        }
        public GetChannelKeyHandler(IUniSpySession session, IUniSpyRequest request) : base(session, request)
        {
            _result = new GetChannelKeyResult();
        }

        protected override void DataOperation()
        {
            _result.ChannelUserIRCPrefix = _user.UserInfo.IRCPrefix;
            _result.Values = _channel.Property.GetChannelValueString(_request.Keys);
            _result.ChannelName = _channel.Property.ChannelName;
        }

        protected override void ResponseConstruct()
        {
            _response = new GetChannelKeyResponse(_request, _result);
        }
    }
}
