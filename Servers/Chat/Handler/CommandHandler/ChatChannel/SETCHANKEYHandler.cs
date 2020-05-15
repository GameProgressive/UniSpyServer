using Chat.Entity.Structure;
using Chat.Entity.Structure.ChatCommand;
using Chat.Entity.Structure.ChatResponse;
using GameSpyLib.Common.Entity.Interface;

namespace Chat.Handler.CommandHandler
{
    public class SETCHANKEYHandler : ChatJoinedChannelHandlerBase
    {
        new SETCHANKEY _cmd;

        public SETCHANKEYHandler(ISession client, ChatCommandBase cmd) : base(client, cmd)
        {
            _cmd = (SETCHANKEY)cmd;
        }


        public override void DataOperation()
        {
            base.DataOperation();
            if (!_user.IsChannelOperator)
            {
                _errorCode = ChatError.NotChannelOperator;
                return;
            }
            _channel.Property.SetChannelKeyValue(_cmd.KeyValue);
        }

        public override void ConstructResponse()
        {
            base.ConstructResponse();
            string flags = "";
            foreach (var kv in _cmd.KeyValue)
            {
                flags += $@"\{kv.Key}\{kv.Value}";
            }
            _sendingBuffer =
                ChatReply.BuildGetChanKeyReply(
                    _user, _channel.Property.ChannelName, "BCAST", flags);
        }

    }
}
