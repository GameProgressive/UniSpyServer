using Chat.Abstraction.BaseClass;
using Chat.Entity.Structure;
using Chat.Entity.Structure.ChatCommand;
using Chat.Entity.Structure.Response.Channel;
using UniSpyLib.Abstraction.Interface;

namespace Chat.Handler.CmdHandler.Channel
{
    // Sets channel key/values.
    // If user is NULL or "", the keys will be set on the channel.
    // Otherwise, they will be set on the user,
    // Only ops can set channel keys on other users.
    // Set a value to NULL or "" to clear that key.
    public class SETCHANKEYHandler : ChatChannelHandlerBase
    {
        protected new SETCHANKEYRequest _request { get { return (SETCHANKEYRequest)base._request; } }

        public SETCHANKEYHandler(IUniSpySession session, IUniSpyRequest request) : base(session, request)
        {
        }

        protected override void DataOperation()
        {
            base.DataOperation();
            if (!_user.IsChannelOperator)
            {
                _errorCode = ChatErrorCode.NotChannelOperator;
                return;
            }
            _channel.Property.SetChannelKeyValue(_request.KeyValue);
        }

        protected override void BuildNormalResponse()
        {
            base.BuildNormalResponse();
            string flags = "";
            foreach (var kv in _request.KeyValue)
            {
                flags += $@"\{kv.Key}\{kv.Value}";
            }
            _sendingBuffer =
                GETCHANKEYResponse.BuildGetChanKeyReply(
                    _user, _channel.Property.ChannelName, "BCAST", flags);
        }

    }
}
