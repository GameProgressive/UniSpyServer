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
            Property.SetProperties(creator,cmd);
            creator.ClientInfo.JoinedChannels.Add(this);
        }
        /// <summary>
        /// Send message to all users in this channel
        /// except the sender
        /// </summary>
        /// <returns></returns>
        public virtual bool MultiCast(ChatSession sender, string message)
        {
            var others = Property.ChannelUsers.Where(user => user != sender);

            foreach (var o in others)
            {
                o.SendAsync(message);
            }

            return true;
        }

        public virtual bool JoinChannel(ChatSession session)
        {
            if (Property.ChannelMode.ModesKV['i'] == '+')
            {
                //invited only
                return false;
            }
            if (Property.BanList.Where(c => c.Id == session.Id).Count() == 1)
            {
                return false;
            }
            if (Property.ChannelUsers.Where(c => c.Id == session.Id).Count() == 1)
            {
                //this man is already in channel
                return false;
            }
            if (session.ClientInfo.JoinedChannels.Where(c => c.Property.ChannelName == Property.ChannelName).Count() == 0)
                session.ClientInfo.JoinedChannels.Add(this);
            return true;
        }

        public virtual bool LeaveChannel(ChatSession session)
        {
            Property.ChannelUsers.Remove(session);
            return true;
        }

        public virtual bool LeaveChannel(ChatSession session, string reason)
        {
            Property.ChannelUsers.Remove(session);
            string buffer = new PART().BuildResponse(Property.ChannelName);
            MultiCast(session, buffer);
            return false;
        }

        public string GetAvailableCommands()
        {
            throw new NotImplementedException();
        }
    }
}
