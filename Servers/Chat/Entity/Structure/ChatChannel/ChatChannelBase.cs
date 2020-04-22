using System.Linq;
using System.Net;
using Chat.Entity.Structure.ChatCommand;
using Chat.Server;

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
            AddBindOnUserAndChannel(joiner);


            //first we send join information to all user in this channel
            MultiCastJoin(joiner);




            //then we send user list which already in this channel ???????????
            SendChannelUsersToJoiner(joiner);

            //send channel mode to joiner
            SendChannelModesToJoiner(joiner);

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

        protected void MultiCastJoin(ChatSession joiner)
        {
            string joinMessage =
                   ChatCommandBase.BuildMessageRPL(
                       $"{joiner.ClientInfo.NickName}!{joiner.ClientInfo.UserName}@{((IPEndPoint)joiner.Socket.RemoteEndPoint).Address}",
                       "JOIN", $"{Property.ChannelName}");


            string modes = Property.ChannelMode.GetChannelMode();
            joinMessage += ChatCommandBase.BuildMessageRPL(
                                          $"{ChatServer.ServerDomain}", $"MODE {Property.ChannelName} {modes}", "");

            MultiCast(joinMessage);
        }
        protected void MultiCastLeave(ChatSession leaver, string message)
        {
            string leaveMessage = ChatCommandBase.BuildMessageRPL(
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

        public void SendChannelUsersToJoiner(ChatSession joiner)
        {
            string modes = Property.ChannelMode.GetChannelMode();

            string buffer = ChatCommandBase.BuildMessageRPL(
                ChatServer.ServerDomain, $"MODE {Property.ChannelName} {modes}", "");

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

            //check the message :@<nickname> whether broadcast char @ ?
            buffer += ChatCommandBase.BuildNormalRPL("www.rspy.cc",
               ChatResponseType.NameReply,
               $"{ joiner.ClientInfo.NickName} = { Property.ChannelName}",
               $"{nicks}");
            buffer += ChatCommandBase.BuildNormalRPL("www.rspy.cc",
                ChatResponseType.EndOfNames,
                $"{joiner.ClientInfo.NickName} {Property.ChannelName}",
                @"End of /NAMES list.");

            joiner.SendAsync(buffer);
        }
        public void SendChannelModesToJoiner(ChatSession joiner)
        {
            string modes = Property.ChannelMode.GetChannelMode();
            string modesMessage =
                ChatCommandBase.BuildNormalRPL(
                    ChatServer.ServerDomain,
                    ChatResponseType.ChannelModels,
                    $"{joiner.ClientInfo.NickName} {Property.ChannelName} {modes}", "");

            joiner.SendAsync(modesMessage);
        }


        public void AddBindOnUserAndChannel(ChatSession joiner)
        {
            if (!Property.ChannelUsers.Contains(joiner))
                Property.ChannelUsers.Add(joiner);

            if (!joiner.ClientInfo.JoinedChannels.Contains(this))
                joiner.ClientInfo.JoinedChannels.Add(this);

        }
        public void RemoveBindOnUserAndChannel(ChatSession leaver)
        {
            if (!Property.ChannelUsers.Contains(leaver))
                Property.ChannelUsers.TryTake(out leaver);

            if (!leaver.ClientInfo.JoinedChannels.Contains(this))
                leaver.ClientInfo.JoinedChannels.Remove(this);
        }
    }
}
