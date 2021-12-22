using UniSpyServer.Servers.Chat.Network;
using System.Linq;
using UniSpyServer.UniSpyLib.Logging;
using UniSpyServer.UniSpyLib.Abstraction.Interface;
using System.Collections.Generic;

namespace UniSpyServer.Servers.Chat.Entity.Structure.Misc.ChannelInfo
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
        public bool MultiCast(IUniSpyResponse message)
        {
            foreach (var kv in Property.ChannelUsers)
            {
                kv.Value.UserInfo.Session.Send(message);
            }
            LogWriter.LogNetworkMultiCast((string)message.SendingBuffer);
            return true;
        }
        public bool MultiCastExceptSender(ChannelUser sender, IUniSpyResponse message)
        {
            foreach (var kv in Property.ChannelUsers)
            {
                if (kv.Value.UserInfo.Session.Id == sender.UserInfo.Session.Id)
                {
                    continue;
                }
                kv.Value.UserInfo.Session.Send(message);
            }

            return true;
        }
        public string GetAllUsersNickString()
        {
            string nicks = "";
            foreach (var kv in Property.ChannelUsers)
            {
                if (kv.Value.IsChannelCreator)
                {
                    nicks += "@" + kv.Value.UserInfo.NickName + " ";
                }
                else
                {
                    nicks += kv.Value.UserInfo.NickName + " ";
                }
            }
            //if user equals last user in channel we do not add space after it
            nicks = nicks.Substring(0, nicks.Length - 1);
            return nicks;
        }
        public void AddBindOnUserAndChannel(ChannelUser joiner)
        {
            // !! we can not directly use the Contains() method that ConcurrentDictionary or 
            // !! ConcurrentBag provide because it will not work properly.
            if (!Property.ChannelUsers.Keys.Contains(joiner.UserInfo.NickName))
            {
                Property.ChannelUsers.TryAdd(joiner.UserInfo.NickName, joiner);
            }

            if (!joiner.UserInfo.JoinedChannels.Keys.Contains(this.Property.ChannelName))
            {
                joiner.UserInfo.JoinedChannels.TryAdd(this.Property.ChannelName, this);
            }

        }
        public void RemoveBindOnUserAndChannel(ChannelUser leaver)
        {
            //!! we should use ConcurrentDictionary here
            //!! FIXME: when removing user from channel, 
            //!! we should do more checks on user not only just TryTake()
            if (Property.ChannelUsers.Keys.Contains(leaver.UserInfo.NickName))
            // !! we takeout wrong user from channel
            {
                var kv = new KeyValuePair<string, ChannelUser>(
                    leaver.UserInfo.NickName,
                    Property.ChannelUsers[leaver.UserInfo.NickName]);
                Property.ChannelUsers.TryRemove(kv);
            }

            if (leaver.UserInfo.JoinedChannels.Keys.Contains(this.Property.ChannelName))
            {
                var kv = new KeyValuePair<string, Channel>(this.Property.ChannelName, this);
                leaver.UserInfo.JoinedChannels.TryRemove(kv);
            }

        }

        public ChannelUser GetChannelUserBySession(Session session)
        {
            return Property.ChannelUsers.Values.Where(u => u.UserInfo.Session.Id == session.Id).FirstOrDefault();
        }
        public bool IsUserBanned(ChannelUser user)
        {
            if (!Property.BanList.Keys.Contains(user.UserInfo.NickName))
            {
                return false;
            }
            if (Property.BanList[user.UserInfo.NickName].UserInfo.Session.Id != user.UserInfo.Session.Id)
            {
                return false;
            }
            return true;
        }
        public bool IsUserBanned(Session session)
        {
            if (Property.BanList.Keys.Contains(session.UserInfo.NickName))
            {
                var resultUser = Property.BanList[session.UserInfo.NickName];
                if (resultUser.UserInfo.Session.Id == session.Id)
                {
                    return true;
                }
            }

            return false;
        }
        public bool IsUserExisted(ChannelUser user) => Property.ChannelUsers.Keys.Contains(user.UserInfo.NickName);

        public ChannelUser GetChannelUserByNickName(string nickName)
        {
            if (Property.ChannelUsers.Keys.Contains(nickName))
            {
                return Property.ChannelUsers[nickName];
            }
            else
            {
                return null;
            }
        }
    }
}
