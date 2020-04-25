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

        public void CreateChannel(ChatSession creator, JOIN cmd)
        {
            Property.SetDefaultProperties(creator,cmd);
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
                       $"{joiner.UserInfo.NickName}!{joiner.UserInfo.UserName}@{((IPEndPoint)joiner.Socket.RemoteEndPoint).Address}",
                       "JOIN", $"{Property.ChannelName}");


            string modes = Property.ChannelMode.GetChannelMode();
            joinMessage += ChatCommandBase.BuildMessageRPL(
                                          $"{ChatServer.ServerDomain}", $"MODE {Property.ChannelName} {modes}", "");

            MultiCast(joinMessage);
        }
        protected void MultiCastLeave(ChatSession leaver, string message)
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
                { nicks += "@" + user.UserInfo.NickName + " "; }
                else
                { nicks += user.UserInfo.NickName+" "; }
            }
            //if user equals last user in channel we do not add space after it
            nicks = nicks.Substring(0, nicks.Length - 1);

            //check the message :@<nickname> whether broadcast char @ ?
            buffer += ChatCommandBase.BuildNormalRPL("www.rspy.cc",
               ChatResponseType.NameReply,
               $"{ joiner.UserInfo.NickName} = { Property.ChannelName}",
               $"{nicks}");
            buffer += ChatCommandBase.BuildNormalRPL("www.rspy.cc",
                ChatResponseType.EndOfNames,
                $"{joiner.UserInfo.NickName} {Property.ChannelName}",
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
                    $"{joiner.UserInfo.NickName} {Property.ChannelName} {modes}", "");

            joiner.SendAsync(modesMessage);
        }


        public void AddBindOnUserAndChannel(ChatSession joiner)
        {
            if (!Property.ChannelUsers.Contains(joiner))
                Property.ChannelUsers.Add(joiner);

            if (!joiner.UserInfo.JoinedChannels.Contains(this))
                joiner.UserInfo.JoinedChannels.Add(this);

        }
        public void RemoveBindOnUserAndChannel(ChatSession leaver)
        {
            if (!Property.ChannelUsers.Contains(leaver))
                Property.ChannelUsers.TryTake(out leaver);

            if (!leaver.UserInfo.JoinedChannels.Contains(this))
                leaver.UserInfo.JoinedChannels.Remove(this);
        }
    }
}
