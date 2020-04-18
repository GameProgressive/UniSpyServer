using Chat.Server;
using System;
using System.Collections.Concurrent;
using System.Linq;

namespace Chat.Entity.Structure
{
    public class ChatChannel
    {
        public Guid Channelid { get; protected set; }
        public uint MaxNumberUser { get; protected set; }
        public ChatChannelMode ChannelMode { get; protected set; }
        public ConcurrentDictionary<Guid, ChatSession> Users;

        public ChatChannel()
        {
            MaxNumberUser = 200;
            // ChannelMode = ChatChannelMode.Moderated;
            Channelid = Guid.NewGuid();
            Users = new ConcurrentDictionary<Guid, ChatSession>();
        }
        public ChatChannel(uint maxNumberUser, ChatChannelMode channelMode)
        {
            MaxNumberUser = maxNumberUser;
            ChannelMode = channelMode;
            Channelid = Guid.NewGuid();
            Users = new ConcurrentDictionary<Guid, ChatSession>();
        }

        /// <summary>
        /// Send message to all users in this channel
        /// except the sender
        /// </summary>
        /// <returns></returns>
        public bool MultiCast(ChatSession sender, string message)
        {
            var others = Users.Values.Where(user => user != sender);

            foreach (var o in others)
            {
                o.SendAsync(message);
            }

            return true;
        }

    }
}
