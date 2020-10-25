using Chat.Entity.Structure;
using Chat.Entity.Structure.ChatChannel;
using Chat.Entity.Structure.ChatCommand;
using Chat.Entity.Structure.ChatResponse;
using Chat.Entity.Structure.ChatResponse.ChatGeneralResponse;
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
        ChatChannelBase _resultChannel;
        ChatSession _resultSession;
        public WHOHandler(ISession session, ChatRequestBase request) : base(session, request)
        {
            _request = (WHORequest)request;
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
                _errorCode = ChatError.NoSuchChannel;
                return;
            }
            _resultChannel = channel;

        }
        /// <summary>
        /// Send all channel user information
        /// </summary>
        private void GetUserInfo()
        {
            ChatSession session;
            if (!ChatSessionManager.GetSessionByUserName(_request.NickName, out session))
            {
                _errorCode = ChatError.NoSuchNick;
                return;
            }
            _resultSession = session;
        }

        protected override void BuildNormalResponse()
        {
            base.BuildNormalResponse();
            _sendingBuffer = "";
            switch (_request.RequestType)
            {
                case WHOType.GetChannelUsersInfo:
                    BuildWhoReplyForChannelUser();
                    break;

                case WHOType.GetUserInfo:
                    BuildWhoReplyForUser();
                    break;
            }
        }


        protected override void BuildErrorResponse()
        {
            base.BuildErrorResponse();
            switch (_errorCode)
            {
                case ChatError.NoSuchChannel:
                    _sendingBuffer =
                        ChatIRCError.BuildNoSuchChannelError(_request.ChannelName);
                    break;
                case ChatError.NoSuchNick:
                    _sendingBuffer = ChatIRCError.BuildNoSuchNickError();
                    break;
            }

        }

        private void BuildWhoReplyForChannelUser()
        {
            foreach (var user in _resultChannel.Property.ChannelUsers)
            {
                _sendingBuffer +=
                    WHOReply.BuildWhoReply(
                        _resultChannel.Property.ChannelName,
                        user.UserInfo, user.GetUserModes());
            }
            _sendingBuffer +=
                WHOReply.BuildEndOfWhoReply(_resultChannel.Property.ChannelName);
        }
        private void BuildWhoReplyForUser()
        {
            foreach (var channel in _resultSession.UserInfo.JoinedChannels)
            {
                ChatChannelUser user;
                channel.GetChannelUserBySession(_resultSession, out user);
                _sendingBuffer +=
                    WHOReply.BuildWhoReply(
                        channel.Property.ChannelName,
                        _resultSession.UserInfo,
                        user.GetUserModes());
            }
            _sendingBuffer +=
                WHOReply.BuildEndOfWhoReply(_resultSession.UserInfo.NickName);
        }
    }
}
