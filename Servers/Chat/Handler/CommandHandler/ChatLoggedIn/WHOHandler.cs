using Chat.Entity.Structure;
using Chat.Entity.Structure.ChatChannel;
using Chat.Entity.Structure.ChatCommand;
using Chat.Entity.Structure.ChatResponse;
using Chat.Handler.SystemHandler.ChannelManage;
using Chat.Handler.SystemHandler.ChatSessionManage;
using Chat.Server;
using GameSpyLib.Common.Entity.Interface;

namespace Chat.Handler.CommandHandler.ChatBasicCommandHandler
{
    /// <summary>
    /// Get a channel user's basic information
    /// same as WHOIS
    /// </summary>
    public class WHOHandler : ChatLogedInHandlerBase
    {
        new WHO _cmd;

        public WHOHandler(ISession session, ChatCommandBase cmd) : base(session, cmd)
        {
            _cmd = (WHO)cmd;
        }

        public override void DataOperation()
        {
            base.DataOperation();
            switch (_cmd.RequestType)
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
            if (!ChatChannelManager.GetChannel(_cmd.ChannelName, out channel))
            {
                _errorCode = ChatError.IRCError;
                _sendingBuffer = ChatIRCError.BuildNoSuchChannelError(_cmd.ChannelName);
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

            if (!ChatSessionManager.GetSessionByUserName(_cmd.NickName, out session))
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
