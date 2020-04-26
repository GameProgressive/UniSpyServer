using System.Linq;
using System.Net;
using Chat.Entity.Structure.ChatCommand;
using Chat.Server;
using GameSpyLib.Logging;

namespace Chat.Entity.Structure.ChatChannel
{
    public class ChatChannelBase
    {
        public ChatChannelProperty Property;

        public ChatChannelBase()
        {
            Property = new ChatChannelProperty();
        }

        public void CreateChannel(ChatChannelUser creator, JOIN cmd)
        {
            //simple check for avoiding program crash
            if (Property.ChannelUsers.Count() != 0)
            {
                return;
            }
            Property.SetDefaultProperties(creator, cmd);
        }


        public void JoinChannel(ChatChannelUser joiner)
        {
            //simple check for avoiding program crash
            if (IsUserExisted(joiner))
            {
                return;
            }

            AddBindOnUserAndChannel(joiner);

            //first we send join information to all user in this channel
            MultiCastJoin(joiner);

            //then we send user list which already in this channel ???????????
            SendChannelUsersToJoiner(joiner);

            //send channel mode to joiner
            SendChannelModesToJoiner(joiner);
        }

        public void LeaveChannel(ChatChannelUser leaver, string message)
        {
            if (!IsUserExisted(leaver))
            {
                return;
            }

            RemoveBindOnUserAndChannel(leaver);

            MultiCastLeave(leaver, message);

            return;
        }

        protected void MultiCastJoin(ChatChannelUser joiner)
        {
            string ip = ((IPEndPoint)joiner.Session.Socket.RemoteEndPoint).Address.ToString();
            string joinMessage =
                   ChatCommandBase.BuildMessageRPL(
                       $"{joiner.UserInfo.NickName}!{joiner.UserInfo.UserName}@{ip}",
                       "JOIN", $"{Property.ChannelName}");


            string modes = Property.ChannelMode.GetChannelMode();
            joinMessage += ChatCommandBase.BuildMessageRPL(
                                          $"{ChatServer.ServerDomain}", $"MODE {Property.ChannelName} {modes}", "");

            MultiCast(joinMessage);
        }

        protected void MultiCastLeave(ChatChannelUser leaver, string message)
        {
            string leaveMessage = ChatCommandBase.BuildMessageRPL(
                $"{leaver.UserInfo.NickName}!{leaver.UserInfo.UserName}@{ChatServer.ServerDomain}",
                $"PART {Property.ChannelName}", message);
            MultiCast(leaveMessage);
        }
        /// <summary>
        /// Send message to all users in this channel
        /// except the sender
        /// </summary>
        /// <returns></returns>
        public virtual bool MultiCast(string message)
        {
            foreach (var o in Property.ChannelUsers)
            {
                o.Session.SendAsync(message);
            }

            return true;
        }

        public void SendChannelUsersToJoiner(ChatChannelUser joiner)
        {
            string modes = Property.ChannelMode.GetChannelMode();

            string buffer = ChatCommandBase.BuildMessageRPL(
                ChatServer.ServerDomain, $"MODE {Property.ChannelName} {modes}", "");

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

            //check the message :@<nickname> whether broadcast char @ ?
            buffer += ChatCommandBase.BuildNormalRPL(
               ChatResponseType.NameReply,
               $"{joiner.UserInfo.NickName} = {Property.ChannelName}",
               $"{nicks}");
            buffer += ChatCommandBase.BuildNormalRPL(
                ChatResponseType.EndOfNames,
                $"{joiner.UserInfo.NickName} {Property.ChannelName}",
                @"End of /NAMES list.");

            joiner.Session.SendAsync(buffer);
        }

        public void SendChannelModesToJoiner(ChatChannelUser joiner)
        {
            string modes = Property.ChannelMode.GetChannelMode();
            string modesMessage =
                ChatCommandBase.BuildNormalRPL(
                    ChatServer.ServerDomain,
                    ChatResponseType.ChannelModels,
                    $"{joiner.UserInfo.NickName} {Property.ChannelName} {modes}", "");

            joiner.Session.SendAsync(modesMessage);
        }


        protected void AddBindOnUserAndChannel(ChatChannelUser joiner)
        {
            if (!Property.ChannelUsers.Contains(joiner))
                Property.ChannelUsers.Add(joiner);

            if (!joiner.UserInfo.JoinedChannels.Contains(this))
                joiner.UserInfo.JoinedChannels.Add(this);

        }
        protected void RemoveBindOnUserAndChannel(ChatChannelUser leaver)
        {
            if (Property.ChannelUsers.Contains(leaver))
                Property.ChannelUsers.TryTake(out leaver);

            if (leaver.UserInfo.JoinedChannels.Contains(this))
                leaver.UserInfo.JoinedChannels.Remove(this);
        }

        public bool GetChannelUser(ChatSession session, out ChatChannelUser user)
        {
            var result = Property.ChannelUsers.Where(u => u.Session.Equals(session));
            if (result.Count() == 1)
            {
                user = result.First();
                return true;
            }
            user = null;
            return false;
        }

        public bool IsUserBanned(ChatChannelUser user)
        {
            if (Property.BanList.Where(u => u.Session.Equals(user.Session)).Count() == 1)
            {
                return true;
            }
            return false;
        }
        public bool IsUserExisted(ChatChannelUser user)
        {
            if (Property.ChannelUsers.Where(u => u.Session.Equals(user.Session)).Count() == 1)
            {
                return true;
            }
            return false;
        }
    }
}
