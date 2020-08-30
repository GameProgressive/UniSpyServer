using System;
using Chat.Entity.Structure;
using Chat.Entity.Structure.ChatChannel;
using Chat.Entity.Structure.ChatCommand;
using Chat.Entity.Structure.ChatResponse;
using GameSpyLib.Common.Entity.Interface;

namespace Chat.Handler.CommandHandler
{
    public class ChatChannelHandlerBase : ChatLogedInHandlerBase
    {
        protected ChatChannelBase _channel;
        protected ChatChannelUser _user;
        new ChatChannelRequestBase _request;

        public ChatChannelHandlerBase(ISession session, ChatRequestBase request) : base(session, request)
        {
            _request = new ChatChannelRequestBase(request.RawRequest);
        }

        protected override void CheckRequest()
        {
            base.CheckRequest();

            if (_errorCode != ChatError.NoError)
            {
                return;
            }

            if (_session.UserInfo.JoinedChannels.Count == 0)
            {
                _errorCode = ChatError.IRCError;
                _sendingBuffer = ChatIRCError.BuildNoSuchChannelError(_request.ChannelName);
                return;
            }

            if (!_session.UserInfo.GetJoinedChannelByName(_request.ChannelName, out _channel))
            {
                _errorCode = ChatError.IRCError;
                _sendingBuffer = ChatIRCError.BuildNoSuchChannelError(_request.ChannelName);
                return;
            }

            if (!_channel.GetChannelUserBySession(_session, out _user))
            {
                _errorCode = ChatError.Parse;
                _sendingBuffer = ChatIRCError.BuildNoSuchNickError();
                return;
            }

        }
    }
}
