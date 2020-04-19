using Chat.Entity.Structure.ChatCommand;
using Chat.Server;
using System;
using System.Linq;

namespace Chat.Entity.Structure.ChatChannel
{
    public class ChatChannelBase
    {
        public ChatChannelProperty Property;

        public ChatChannelBase(ChatSession creator)
        {
            Property = new ChatChannelProperty(creator);
        }
        public ChatChannelBase(ChatSession creator, DateTime lifeTime)
        {
            Property = new ChatChannelProperty(creator, lifeTime);
            // ChannelMode = ChatChannelMode.Moderated;
        }

        public ChatChannelBase(ChatSession creator, DateTime lifeTime, uint maxUser)
        {
            Property = new ChatChannelProperty(creator, lifeTime, maxUser);
        }

        public bool Create(ChatCommandBase cmd)
        {
            return Property.SetProperties(cmd);
        }
        /// <summary>
        /// Send message to all users in this channel
        /// except the sender
        /// </summary>
        /// <returns></returns>
        public virtual bool MultiCast(ChatSession sender, string message)
        {
            var others = Property.ChannelUsers.Values.Where(user => user != sender);

            foreach (var o in others)
            {
                o.SendAsync(message);
            }

            return true;
        }

        public virtual bool JoinChannel(ChatSession session)
        {
            return false;
        }
        public virtual bool JoinChannel(ChatSession session, string reason)
        {
            return false;
        }

        public virtual bool LeaveChannel(ChatSession session)
        {
            Property.ChannelUsers.TryRemove(session.Id, out _);
            return true;
        }

        public virtual bool LeaveChannel(ChatSession session, string reason)
        {
            return false;
        }

        public string GetAvailableCommands()
        {
            throw new NotImplementedException();
        }
    }
}
