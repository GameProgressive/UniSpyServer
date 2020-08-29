using Chat.Entity.Structure;
using Chat.Entity.Structure.ChatCommand;
using Chat.Entity.Structure.ChatResponse;
using GameSpyLib.Common.Entity.Interface;

namespace Chat.Handler.CommandHandler.ChatChannelCommandHandler
{
    // Sets channel key/values.
    // If user is NULL or "", the keys will be set on the channel.
    // Otherwise, they will be set on the user,
    // Only ops can set channel keys on other users.
    // Set a value to NULL or "" to clear that key.
    public class SETCHANKEYHandler : ChatChannelHandlerBase
    {
        new SETCHANKEYRequest _request;

        public SETCHANKEYHandler(ISession session, ChatRequestBase request) : base(session, request)
        {
            _request = (SETCHANKEYRequest)request;
        }


        protected override void DataOperation()
        {
            base.DataOperation();
            if (!_user.IsChannelOperator)
            {
                _errorCode = ChatError.NotChannelOperator;
                return;
            }
            _channel.Property.SetChannelKeyValue(_request.KeyValue);
        }

        protected override void ConstructResponse()
        {
            base.ConstructResponse();
            string flags = "";
            foreach (var kv in _request.KeyValue)
            {
                flags += $@"\{kv.Key}\{kv.Value}";
            }
            _sendingBuffer =
                ChatReply.BuildGetChanKeyReply(
                    _user, _channel.Property.ChannelName, "BCAST", flags);
        }

    }
}
