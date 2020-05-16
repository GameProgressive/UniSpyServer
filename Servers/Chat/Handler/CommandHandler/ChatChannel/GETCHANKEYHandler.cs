using System;
using Chat.Entity.Structure.ChatCommand;
using Chat.Entity.Structure.ChatResponse;
using GameSpyLib.Common.Entity.Interface;

namespace Chat.Handler.CommandHandler
{
    public class GETCHANKEYHandler : ChatJoinedChannelHandlerBase
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
