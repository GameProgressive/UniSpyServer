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
            BuildEndOfWhoReply();
        }

        private void GetChannelUsersInfo()
        {
            ChatChannelBase channel;
            if (!ChatChannelManager.GetChannel(_cmd.Name, out channel))
            {
                _errorCode = ChatError.IRCError;
                return;
            }
            _sendingBuffer = "";

            foreach (var user in channel.Property.ChannelUsers)
            {
                _sendingBuffer +=
                    ChatReply.BuildWhoReply(
                        channel.Property.ChannelName,
                        user.UserInfo,user.GetUserModes());
            }
        }

        /// <summary>
        /// Send all channel user information
        /// </summary>
        private void GetUserInfo()
        {
            ChatSession session;

            if (ChatSessionManager.GetSessionByNickName(_cmd.Name, out session))
            {
                BuildWhoReplyForUser(session);
            }
            else if (ChatSessionManager.GetSessionByUserName(_cmd.Name, out session))
            {
                BuildWhoReplyForUser(session);
            }
            else //todo check whether we need this error
            {
                _errorCode = ChatError.IRCError;
                _sendingBuffer = ChatIRCError.BuildNoSuchNickError();
            }
        }

        private void BuildEndOfWhoReply()
        {
            _sendingBuffer += ChatReply.BuildEndOfWhoReply(_session.UserInfo);
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
        }
    }
}
