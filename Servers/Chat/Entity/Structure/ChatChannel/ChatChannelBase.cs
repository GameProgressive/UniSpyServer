using Chat.Entity.Structure.ChatCommand;
using Chat.Server;
using System;
using System.Linq;

namespace Chat.Entity.Structure.ChatChannel
{
    public class ChatChannelBase
    {
        public ChatChannelProperty Property;

        public ChatChannelBase()
        {
            Property = new ChatChannelProperty();
        }

        public void CreateChannel(ChatSession creator, ChatCommandBase cmd)
        {
            Property.SetProperties(creator, cmd);
            JoinChannel(creator);
        }


        public bool JoinChannel(ChatSession joiner)
        {
            if (Property.ChannelMode.ModesKV['i'] == '+')
            {
                //invited only
                return false;
            }
            if (Property.BanList.Where(c => c.Id == joiner.Id).Count() == 1)
            {
                return false;
            }

            if (Property.ChannelUsers.Where(u => u.Equals(joiner)).Count() != 0)
            {
                //already in channel
                return false;
            }
            //first we send join information to all user in this channel
            MultiCastJoin(joiner);

            AddBindOnUserAndChannel(joiner);
            //send channel mode to joiner
            SendChannelModes(joiner);

            //then we send user list which already in this channel ???????????
            SendChannelUsers(joiner);

            return true;
        }

        public bool LeaveChannel(ChatSession leaver, string message)
        {
            if (!Property.ChannelUsers.Contains(leaver))
            {
                return false;
            }

            RemoveBindOnUserAndChannel(leaver);

            MultiCastLeave(leaver, message);

            return true;
        }

        public void SendChannelModes(ChatSession joiner)
        {
            string modes = Property.ChannelMode.GetChannelMode();
            string modesMessage = ChatCommandBase.BuildMessage(
                $"{ChatServer.ServerDomain}", $"MODE {Property.ChannelName} {modes}", "");
            joiner.SendAsync(modesMessage);
        }

        protected void MultiCastJoin(ChatSession joiner)
        {
            string joinMessage =
                   ChatCommandBase.BuildMessage(
                       $"{joiner.ClientInfo.NickName}!{joiner.ClientInfo.UserName}@{ChatServer.ServerDomain}",
                       "JOIN", $"{Property.ChannelName}");

            MultiCast(joinMessage);
        }

        protected void MultiCastLeave(ChatSession leaver, string message)
        {
            string leaveMessage = ChatCommandBase.BuildMessage(
                $"{leaver.ClientInfo.NickName}!{leaver.ClientInfo.UserName}@{ChatServer.ServerDomain}",
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
                o.SendAsync(message);
            }

            return true;
        }

        public void SendChannelUsers(ChatSession joiner)
        {
            string nicks = "";
            foreach (var user in Property.ChannelUsers)
            {
                if (user == Property.ChannelCreator)
                { nicks += "@" + user.ClientInfo.NickName + " "; }
                else
                { nicks += user.ClientInfo.NickName; }
            }
            //if user equals last user in channel we do not add space after it
            nicks = nicks.Substring(0, nicks.Length - 1);

            string buffer = ChatCommandBase.BuildRPL("www.rspy.cc",
                  ChatResponseType.NameReply,
                  $"{ joiner.ClientInfo.NickName} = { Property.ChannelName}",
                  $"{nicks}");
            buffer += ChatCommandBase.BuildRPL("www.rspy.cc",
                ChatResponseType.EndOfNames,
                $"{joiner.ClientInfo.NickName} {Property.ChannelName}",
                "End of /NAMES list.");
            //string buffer = $":irc.foonet.com 353 {joiner.ClientInfo.NickName} = {Property.ChannelName} :{nicks}\r\n";
            //buffer += $":irc.foonet.com 366 {joiner.ClientInfo.NickName} {Property.ChannelName} :End of /NAMES list.\r\n";
            joiner.SendAsync(buffer);
        }

        public void AddBindOnUserAndChannel(ChatSession joiner)
        {
            Property.ChannelUsers.Add(joiner);
            joiner.ClientInfo.JoinedChannels.Add(this);
        }

        public void RemoveBindOnUserAndChannel(ChatSession leaver)
        {
            Property.ChannelUsers.Remove(leaver);
            leaver.ClientInfo.JoinedChannels.Remove(this);
        }


    }
}
