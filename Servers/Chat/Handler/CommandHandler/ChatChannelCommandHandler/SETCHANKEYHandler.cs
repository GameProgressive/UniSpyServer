using System;
using Chat.Entity.Structure.ChatChannel;
using Chat.Entity.Structure.ChatCommand;
using GameSpyLib.Common.Entity.Interface;

namespace Chat.Handler.CommandHandler
{
    public class SETCHANKEYHandler : ChatCommandHandlerBase
    {
        new SETCHANKEY _cmd;
        ChatChannelBase _channel;
        ChatChannelUser _user;
        public SETCHANKEYHandler(ISession client, ChatCommandBase cmd) : base(client, cmd)
        {
            _cmd = (SETCHANKEY)cmd;
        }

        public override void CheckRequest()
        {
            base.CheckRequest();

            if (!_session.UserInfo.GetJoinedChannelByName(_cmd.ChannelName, out _channel))
            {
                _errorCode = Entity.Structure.ChatError.Parse;
                return;
            }
            if (!_channel.GetChannelUserBySession(_session, out _user))
            {
                _errorCode = Entity.Structure.ChatError.Parse;
                return;
            }
        }

        public override void DataOperation()
        {
            base.DataOperation();
            if (!_user.IsChannelOperator)
            {
                _errorCode = Entity.Structure.ChatError.NotChannelOperator;
                return;
            }
            _channel.Property.SetChannelKeyValue(_cmd.KeyValue);
        }
    }
}
