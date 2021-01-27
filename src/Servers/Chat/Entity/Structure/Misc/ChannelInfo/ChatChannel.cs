using Chat.Entity.Structure.Response.General;
using Chat.Handler.SystemHandler.ChannelManage;
using Chat.Network;
using QueryReport.Entity.Structure;
using System.Collections.Concurrent;
using System.Linq;

namespace Chat.Entity.Structure.Misc.ChannelInfo
{
    internal sealed class ChatChannel
    {
        public ChatChannelProperty Property { get; private set; }
        public ConcurrentBag<ChatChannelUser> BanList { get; set; }
        public ConcurrentBag<ChatChannelUser> ChannelUsers { get; set; }
        public ChatChannel()
        {
            Property = new ChatChannelProperty();
            BanList = new ConcurrentBag<ChatChannelUser>();
            ChannelUsers = new ConcurrentBag<ChatChannelUser>();
        }

        /// <summary>
        /// Send message to all users in this channel
        /// except the sender
        /// </summary>
        /// <returns></returns>
        public bool MultiCast(string message)
        {
            foreach (var user in ChannelUsers)
            {
                user.UserInfo.Session.SendAsync(message);
            }

            return true;
        }
        public bool MultiCastExceptSender(ChatChannelUser sender, string message)
        {
            foreach (var user in ChannelUsers)
            {
                if (user.UserInfo.Session.Id == sender.UserInfo.Session.Id)
                {
                    continue;
                }
                user.UserInfo.Session.SendAsync(message);
            }

            return true;
        }
        public string GetAllUsersNickString()
        {
            string nicks = "";
            foreach (var user in ChannelUsers)
            {
                if (user.IsChannelCreator)
                {
                    nicks += "@" + user.UserInfo.NickName + " ";
                }
                else
                {
                    nicks += user.UserInfo.NickName + " ";
                }
            }
            //if user equals last user in channel we do not add space after it
            nicks = nicks.Substring(0, nicks.Length - 1);
            return nicks;
        }
        public void AddBindOnUserAndChannel(ChatChannelUser joiner)
        {
            if (!ChannelUsers.Contains(joiner))
                ChannelUsers.Add(joiner);

            if (!joiner.UserInfo.JoinedChannels.Contains(this))
                joiner.UserInfo.JoinedChannels.Add(this);

        }
        public void RemoveBindOnUserAndChannel(ChatChannelUser leaver)
        {
            if (ChannelUsers.Contains(leaver))
                ChannelUsers.TryTake(out leaver);

            if (leaver.UserInfo.JoinedChannels.Contains(this))
            {
                ChatChannel channel = this;
                leaver.UserInfo.JoinedChannels.TryTake(out channel);
            }

        }

        public ChatChannelUser GetChannelUserBySession(ChatSession session)
        {
            var result = ChannelUsers.Where(u => u.UserInfo.Session.Equals(session));
            if (result.Count() == 1)
            {
                return result.First();
            }
            else
            {
                return null;
            }
        }
        public bool IsUserBanned(ChatChannelUser user)
        {
            if (BanList.Contains(user))
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        public bool IsUserBanned(ChatSession session)
        {
            if (BanList.Where(u => u.UserInfo.Session.Id == session.Id).Count() == 1)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        public bool IsUserExisted(ChatChannelUser user)
        {
            return ChannelUsers.Contains(user);
        }
        public ChatChannelUser GetChannelUserByUserName(string username)
        {
            var result = ChannelUsers.Where(u => u.UserInfo.UserName == username);
            if (result.Count() == 1)
            {
                return result.First();
            }
            else
            {
                return null;
            }
        }
        public ChatChannelUser GetChannelUserByNickName(string nickName)
        {
            var result = ChannelUsers.Where(u => u.UserInfo.NickName == nickName);
            if (result.Count() == 1)
            {
                return result.First();
            }
            else
            {
                return null;
            }
        }

        /// <summary>
        /// because Join channel has its own command class
        /// so we do not add join channel inside this class
        /// leave channel do not have its IRC command and used many places
        /// </summary>
        /// <param name="session"></param>
        public void LeaveChannel(ChatSession session, string reason)
        {
            ChatChannelUser user = GetChannelUserBySession(session);

            if (user == null)
            {
                return;
            }
            LeaveChannel(user, reason);
        }

        public void LeaveChannel(ChatChannelUser user, string reason)
        {
            if (Property.IsPeerServer && user.IsChannelCreator)
            {
                KickAllUserAndShutDownChannel(user);
            }
            else
            {
                MultiCastLeave(user, reason);
            }

            RemoveBindOnUserAndChannel(user);
        }
        public void MultiCastLeave(ChatChannelUser leaver, string message)
        {
            string leaveMessage = ChatIRCReplyBuilder.Build(
                leaver.UserInfo.IRCPrefix,
                ChatReplyName.PART,
                Property.ChannelName,
                message);

            MultiCast(leaveMessage);
        }
        private void KickAllUserAndShutDownChannel(ChatChannelUser kicker)
        {
            foreach (var user in ChannelUsers)
            {
                //kick all user out
                string cmdParams = $"{Property.ChannelName} {kicker.UserInfo.NickName} {user.UserInfo.NickName}";
                string message = "Server Hoster leaves channel";
                string kickMsg = ChatIRCReplyBuilder.Build(ChatReplyName.KICK, cmdParams, message);
                user.UserInfo.Session.SendAsync(kickMsg);
            }

            ChatChannelManager.RemoveChannel(this);
            var fullKey = GameServerInfo.RedisOperator.BuildFullKey(
                kicker.UserInfo.Session.RemoteIPEndPoint,
                kicker.UserInfo.Session.UserInfo.GameName);

            GameServerInfo.RedisOperator.DeleteKeyValue(fullKey);
        }
    }
}
