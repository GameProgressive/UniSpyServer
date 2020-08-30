using Chat.Entity.Structure;
using Chat.Entity.Structure.ChatChannel;
using Chat.Entity.Structure.ChatCommand;
using Chat.Entity.Structure.ChatResponse;
using Chat.Handler.SystemHandler.ChannelManage;
using Chat.Handler.SystemHandler.ChatSessionManage;
using Chat.Server;
using GameSpyLib.Common.Entity.Interface;

namespace Chat.Handler.CommandHandler.ChatGeneralCommandHandler
{
    /// <summary>
    /// Get a channel user's basic information
    /// same as WHOIS
    /// </summary>
    public class WHOHandler : ChatLogedInHandlerBase
    {
        new WHORequest _request;

        public WHOHandler(ISession session, ChatRequestBase request) : base(session, request)
        {
            _request = new WHORequest(request.RawRequest);
        }
        protected override void CheckRequest()
        {
            base.CheckRequest();
            if (!_request.Parse())
            {
                _errorCode = ChatError.Parse;
                return;
            }
        }
        protected override void DataOperation()
        {
            base.DataOperation();
            switch (_request.RequestType)
            {
                case WHOType.GetChannelUsersInfo:
                    GetChannelUsersInfo();
                    break;
                case WHOType.GetUserInfo:
                    GetUserInfo();
                    break;
            }
        }

        private void GetChannelUsersInfo()
        {
            ChatChannelBase channel;
            if (!ChatChannelManager.GetChannel(_request.ChannelName, out channel))
            {
                _errorCode = ChatError.IRCError;
                _sendingBuffer = ChatIRCError.BuildNoSuchChannelError(_request.ChannelName);
                return;
            }
            _sendingBuffer = "";

            foreach (var user in channel.Property.ChannelUsers)
            {
                _sendingBuffer +=
                    ChatReply.BuildWhoReply(
                        channel.Property.ChannelName,
                        user.UserInfo, user.GetUserModes());
            }
            _sendingBuffer +=
                ChatReply.BuildEndOfWhoReply(channel.Property.ChannelName);
        }

        /// <summary>
        /// Send all channel user information
        /// </summary>
        private void GetUserInfo()
        {
            ChatSession session;

            if (!ChatSessionManager.GetSessionByUserName(_request.NickName, out session))
            {
                _errorCode = ChatError.IRCError;
                _sendingBuffer = ChatIRCError.BuildNoSuchNickError();
                return;
            }

            BuildWhoReplyForUser(session);
        }

        private void BuildWhoReplyForUser(ChatSession session)
        {
            _sendingBuffer = "";
            foreach (var channel in session.UserInfo.JoinedChannels)
            {
                ChatChannelUser user;
                channel.GetChannelUserBySession(session, out user);
                _sendingBuffer +=
                    ChatReply.BuildWhoReply(
                        channel.Property.ChannelName,
                        session.UserInfo,
                        user.GetUserModes());
            }
            _sendingBuffer +=
                ChatReply.BuildEndOfWhoReply(session.UserInfo.NickName);
        }
    }
}
