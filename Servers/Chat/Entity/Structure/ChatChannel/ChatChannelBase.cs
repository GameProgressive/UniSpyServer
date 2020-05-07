using System.Linq;
using System.Net;
using Chat.Entity.Structure.ChatCommand;
using Chat.Handler.SystemHandler.ChannelManage;
using Chat.Server;

namespace Chat.Entity.Structure.ChatChannel
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

        public void MultiCastLeave(ChatChannelUser leaver, string message)
        {
            string leaveMessage = leaver.BuildChannelMessage($"PART {Property.ChannelName}", message);
                //ChatCommandBase.BuildMessageRPL(
                //$"{leaver.UserInfo.NickName}!{leaver.UserInfo.UserName}@{leaver.Session.Socket.}",
                //$"PART {Property.ChannelName}", message);
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

            if (nicks.Length < 3)
            {
                return;
            }
            //if user equals last user in channel we do not add space after it
            nicks = nicks.Substring(0, nicks.Length - 1);

            //check the message :@<nickname> whether broadcast char @ ?
            buffer += ChatCommandBase.BuildNumericRPL(
               ChatResponseType.NameReply,
               $"{joiner.UserInfo.NickName} = {Property.ChannelName}",
               $"{nicks}");
            buffer += ChatCommandBase.BuildNumericRPL(
                ChatResponseType.EndOfNames,
                $"{joiner.UserInfo.NickName} {Property.ChannelName}",
                @"End of /NAMES list.");

            joiner.Session.SendAsync(buffer);
        }

        public void SendChannelModesToJoiner(ChatChannelUser joiner)
        {
            string modes = Property.ChannelMode.GetChannelMode();
            string modesMessage =
                ChatCommandBase.BuildNumericRPL(
                    ChatServer.ServerDomain,
                    ChatResponseType.ChannelModels,
                    $"{joiner.UserInfo.NickName} {Property.ChannelName} {modes}", "");

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
                leaver.UserInfo.JoinedChannels.Remove(this);
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
        public void LeaveChannel(ChatSession session,string reason)
        {
            ChatChannelUser user;

            if (!GetChannelUserBySession(session, out user))
            {
                return;
            }
            LeaveChannel(user,reason);
        }

        public void LeaveChannel(ChatChannelUser user,string reason)
        {
            RemoveBindOnUserAndChannel(user);

            if (Property.IsPeerServer)
            {
                RemoveAllUserAndShutDownChannel(user);
            }
            else
            {
                MultiCastLeave(user, reason);
            }
        }

        private void RemoveAllUserAndShutDownChannel(ChatChannelUser kicker)
        {
            foreach (var user in Property.ChannelUsers)
            {
                //kick all user out
                string kickMsg =
                    KICK.BuildKickMessage(this,kicker,user,"Creator leaves channel");
                user.Session.SendAsync(kickMsg);
            }
            ChatChannelManager.RemoveChannel(this);
        }
    }
}
