using Chat.Entity.Structure.ChatCommand;
using Chat.Entity.Structure.ChatResponse;
using GameSpyLib.Common.Entity.Interface;

namespace Chat.Handler.CommandHandler
{
    // Sets channel key/values.
    // If user is NULL or "", the keys will be set on the channel.
    // Otherwise, they will be set on the user,
    // Only ops can set channel keys on other users.
    // Set a value to NULL or "" to clear that key.
    public class GETCHANKEYHandler : ChatChannelHandlerBase
    {
        new GETCHANKEY _cmd;
        string _values;
        public GETCHANKEYHandler(ISession session, ChatCommandBase cmd) : base(session, cmd)
        {
            _cmd = (GETCHANKEY)cmd;
        }

        public override void DataOperation()
        {
            base.DataOperation();
            _values = _channel.Property.GetChannelValueString(_cmd.Keys);
        }

        public override void ConstructResponse()
        {
            base.ConstructResponse();
            _sendingBuffer =
                ChatReply.BuildGetChanKeyReply(
                    _user, _channel.Property.ChannelName,
                    _cmd.Cookie, _values);
        }
    }
}
