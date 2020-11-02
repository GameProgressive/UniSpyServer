using Chat.Entity.Structure.ChatChannel;
using Chat.Entity.Structure.Response.Channel;
using Chat.Entity.Structure.Response.General;
using Chat.Handler.SystemHandler.ChannelManage;
using Chat.Server;
using QueryReport.Entity.Structure;
using System.Linq;

namespace Chat.Abstraction.BaseClass
{
    public class ChatChannelBase
    {
        public ChatChannelProperty Property { get; protected set; }

        public ChatChannelBase()
        {
            Property = new ChatChannelProperty();
        }

        public void MultiCastJoin(ChatChannelUser joiner)
        {
            string joinMessage =
                JOINReply.BuildJoinReply(
                    joiner, Property.ChannelName);

            string modes =
                Property.ChannelMode.GetChannelMode();

            joinMessage +=
                MODEReply.BuildModeReply(
                   Property.ChannelName, modes);

            MultiCast(joinMessage);
        }

        public void MultiCastLeave(ChatChannelUser leaver, string message)
        {
            string leaveMessage =
                PARTReply.BuildPartReply(
                    leaver, Property.ChannelName, message);

            MultiCast(leaveMessage);
        }
        /// <summary>
        /// Send message to all users in this channel
        /// except the sender
        /// </summary>
        /// <returns></returns>
        public virtual bool MultiCast(string message)
        {
            foreach (var user in Property.ChannelUsers)
            {
                user.Session.SendAsync(message);
            }

            return true;
        }

        public virtual bool MultiCastExceptSender(ChatChannelUser sender, string message)
        {
            foreach (var o in Property.ChannelUsers)
            {
                if (o.Session.Id == sender.Session.Id)
                {
                    continue;
                }
                o.Session.SendAsync(message);
            }

            return true;
        }

        public void SendChannelUsersToJoiner(ChatChannelUser joiner)
        {
            string modes = Property.ChannelMode.GetChannelMode();

            string buffer =
                MODEReply.BuildModeReply(
               Property.ChannelName, modes);

            string nicks = "";
            foreach (var user in Property.ChannelUsers)
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

            // crysis2's nickname is * length =1 
            if (nicks.Length < 1)
            {
                return;
            }

            //if user equals last user in channel we do not add space after it
            nicks = nicks.Substring(0, nicks.Length - 1);

            //check the message :@<nickname> whether broadcast char @ ?
            buffer += NAMEReply.BuildNameReply(
                joiner.UserInfo.NickName, Property.ChannelName, nicks);

            buffer += NAMEReply.BuildEndOfNameReply(
                joiner.UserInfo.NickName, Property.ChannelName);

            joiner.Session.SendAsync(buffer);
        }

        public void SendChannelModesToJoiner(ChatChannelUser joiner)
        {
            string modes = Property.ChannelMode.GetChannelMode();

            string modesMessage =
                MODEReply.BuildChannelModesReply(
                    joiner, Property.ChannelName, modes);

            joiner.Session.SendAsync(modesMessage);
        }


        public void AddBindOnUserAndChannel(ChatChannelUser joiner)
        {
            if (!Property.ChannelUsers.Contains(joiner))
                Property.ChannelUsers.Add(joiner);

            if (!joiner.UserInfo.JoinedChannels.Contains(this))
                joiner.UserInfo.JoinedChannels.Add(this);

        }

        public void RemoveBindOnUserAndChannel(ChatChannelUser leaver)
        {
            if (Property.ChannelUsers.Contains(leaver))
                Property.ChannelUsers.TryTake(out leaver);

            if (leaver.UserInfo.JoinedChannels.Contains(this))
            {
                ChatChannelBase channel = this;
                leaver.UserInfo.JoinedChannels.TryTake(out channel);
            }

        }

        public bool GetChannelUserBySession(ChatSession session, out ChatChannelUser user)
        {
            var result = Property.ChannelUsers.Where(u => u.Session.Equals(session));
            if (result.Count() == 1)
            {
                user = result.First();
                return true;
            }
            else
            {
                user = null;
                return false;
            }
        }

        public bool IsUserBanned(ChatChannelUser user)
        {
            if (Property.BanList.Where(u => u.Session.Id == user.Session.Id).Count() == 1)
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
            if (Property.BanList.Where(u => u.Session.Id == session.Id).Count() == 1)
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
            return GetChannelUserBySession(user.Session, out _);
        }

        public bool IsUserExisted(ChatSession session)
        {
            return GetChannelUserBySession(session, out _);
        }

        public bool GetChannelUserByUserName(string username, out ChatChannelUser user)
        {
            var result = Property.ChannelUsers.Where(u => u.Session.UserInfo.UserName == username);
            if (result.Count() == 1)
            {
                user = result.First();
                return true;
            }
            else
            {
                user = null;
                return false;
            }
        }

        public bool GetChannelUserByNickName(string nickname, out ChatChannelUser user)
        {
            var result = Property.ChannelUsers.Where(u => u.Session.UserInfo.NickName == nickname);
            if (result.Count() == 1)
            {
                user = result.First();
                return true;
            }
            else
            {
                user = null;
                return false;
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
            ChatChannelUser user;

            if (!GetChannelUserBySession(session, out user))
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

        private void KickAllUserAndShutDownChannel(ChatChannelUser kicker)
        {
            foreach (var user in Property.ChannelUsers)
            {
                //kick all user out
                string kickMsg =
                    KICKReply.BuildKickReply(
                    Property.ChannelName, kicker, user, "Server Hoster leaves channel");

                user.Session.SendAsync(kickMsg);
            }

            ChatChannelManager.RemoveChannel(this);

            GameServer.DeleteSimilarServer(
                kicker.Session.RemoteEndPoint,
                kicker.Session.UserInfo.GameName);
        }
    }
}
