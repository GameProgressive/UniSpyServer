using System;
using Chat.Entity.Structure;
using Chat.Entity.Structure.ChatChannel;
using Chat.Entity.Structure.ChatCommand;
using Chat.Entity.Structure.ChatResponse;
using GameSpyLib.Common.Entity.Interface;

namespace Chat.Handler.CommandHandler
{
    public class ChatJoinedChannelHandlerBase : ChatLogedInHandlerBase
    {
        ChatChannelBase _channel;
        ChatChannelUser _user;
        new ChatChannelCommandBase _cmd;
        public ChatJoinedChannelHandlerBase(ISession session, ChatCommandBase cmd) : base(session, cmd)
        {
            _cmd = (ChatChannelCommandBase)cmd;
        }

        public override void CheckRequest()
        {
            base.CheckRequest();

            if (_errorCode != ChatError.NoError)
            {
                return;
            }

            if (_session.UserInfo.JoinedChannels.Count == 0)
            {
                _errorCode = ChatError.IRCError;
                return;
            }

            if (!_session.UserInfo.GetJoinedChannel(_cmd.ChannelName, out _channel))
            {
                _errorCode = ChatError.IRCError;
                return;
            }
            if (!_channel.GetChannelUserBySession(_session, out _user))
            {
                _errorCode = ChatError.Parse;
                return;
            }
        }
    }
}
