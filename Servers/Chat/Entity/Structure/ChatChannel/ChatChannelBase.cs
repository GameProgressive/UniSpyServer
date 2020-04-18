using Chat.Server;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;

namespace Chat.Entity.Structure.ChatChannel
{
    public class ChatChannelBase
    {
        public Guid Channelid { get; protected set; }
        public uint MaxNumberUser { get; protected set; }
        public ChatChannelMode ChannelMode { get; protected set; }
        public List<Guid> BanList { get; protected set; }
        public ConcurrentDictionary<Guid, ChatSession> Users;
        public List<ChatSession> ChannelOperators { get; protected set; }
        public ChatSession ChannelCreator { get; protected set; }
        public DateTime ChannelLifeTime { get; protected set; }
        public DateTime ChannelCreatedTime { get; protected set; }

        public ChatChannelBase(ChatSession channelCreator,DateTime lifeTime)
        {
            MaxNumberUser = 200;
            // ChannelMode = ChatChannelMode.Moderated;
            Channelid = Guid.NewGuid();
            BanList = new List<Guid>();
            Users = new ConcurrentDictionary<Guid, ChatSession>();
            ChannelOperators = new List<ChatSession>();
            ChannelCreator = channelCreator;
            ChannelLifeTime = lifeTime;
            ChannelCreatedTime = DateTime.Now;
        }

        public ChatChannelBase(uint maxNumberUser, ChatChannelMode channelMode,DateTime lifeTime,ChatSession channelCreator)
        {
            MaxNumberUser = maxNumberUser;
            ChannelMode = channelMode;
            Channelid = Guid.NewGuid();
            Users = new ConcurrentDictionary<Guid, ChatSession>();
            ChannelCreator = channelCreator;
            ChannelLifeTime = lifeTime;
            ChannelCreatedTime = DateTime.Now;
        }

        /// <summary>
        /// Send message to all users in this channel
        /// except the sender
        /// </summary>
        /// <returns></returns>
        public virtual bool MultiCast(ChatSession sender, string message)
        {
            var others = Users.Values.Where(user => user != sender);

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
            return false;
        }
        public virtual bool LeaveChannel(ChatSession session,string reason)
        {
            return false;
        }
    }
}
