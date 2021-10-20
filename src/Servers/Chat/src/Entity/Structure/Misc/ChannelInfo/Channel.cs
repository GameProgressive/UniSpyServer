using UniSpyServer.Chat.Network;
using System.Linq;
using UniSpyServer.UniSpyLib.Logging;

namespace UniSpyServer.Chat.Entity.Structure.Misc.ChannelInfo
{
    public sealed class Channel
    {
        public ChannelProperty Property { get; private set; }

        public Channel()
        {
            Property = new ChannelProperty();
        }

        /// <summary>
        /// Send message to all users in this channel
        /// except the sender
        /// </summary>
        /// <returns></returns>
        public bool MultiCast(string message)
        {
            foreach (var user in Property.ChannelUsers)
            {
                user.UserInfo.Session.SendAsync(message);
            }
            LogWriter.LogNetworkMultiCast(message);
            return true;
        }
        public bool MultiCastExceptSender(ChannelUser sender, string message)
        {
            foreach (var user in Property.ChannelUsers)
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
            //if user equals last user in channel we do not add space after it
            nicks = nicks.Substring(0, nicks.Length - 1);
            return nicks;
        }
        public void AddBindOnUserAndChannel(ChannelUser joiner)
        {
            if (!Property.ChannelUsers.Contains(joiner))
                Property.ChannelUsers.Add(joiner);

            if (!joiner.UserInfo.JoinedChannels.Contains(this))
                joiner.UserInfo.JoinedChannels.Add(this);

        }
        public void RemoveBindOnUserAndChannel(ChannelUser leaver)
        {
            if (Property.ChannelUsers.Contains(leaver))
                Property.ChannelUsers.TryTake(out leaver);

            if (leaver.UserInfo.JoinedChannels.Contains(this))
            {
                Channel channel = this;
                leaver.UserInfo.JoinedChannels.TryTake(out channel);
            }

        }

        public ChannelUser GetChannelUserBySession(Session session)
        {
            var result = Property.ChannelUsers.Where(u => u.UserInfo.Session.Equals(session));
            if (result.Count() == 1)
            {
                return result.First();
            }
            else
            {
                return null;
            }
        }
        public bool IsUserBanned(ChannelUser user)
        {
            if (Property.BanList.Contains(user))
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        public bool IsUserBanned(Session session)
        {
            if (Property.BanList.Where(u => u.UserInfo.Session.Id == session.Id).Count() == 1)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        public bool IsUserExisted(ChannelUser user)
        {
            return Property.ChannelUsers.Contains(user);
        }
        public ChannelUser GetChannelUserByUserName(string username)
        {
            var result = Property.ChannelUsers.Where(u => u.UserInfo.UserName == username);
            if (result.Count() == 1)
            {
                return result.First();
            }
            else
            {
                return null;
            }
        }
        public ChannelUser GetChannelUserByNickName(string nickName)
        {
            var result = Property.ChannelUsers.Where(u => u.UserInfo.NickName == nickName);
            if (result.Count() == 1)
            {
                return result.First();
            }
            else
            {
                return null;
            }
        }
    }
}
