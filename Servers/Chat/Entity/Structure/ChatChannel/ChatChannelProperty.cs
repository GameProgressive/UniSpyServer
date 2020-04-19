using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using Chat.Entity.Structure.ChatCommand;
using Chat.Server;

namespace Chat.Entity.Structure.ChatChannel
{
    public class ChatChannelProperty
    {
        public Guid Channelid { get; protected set; }
        public uint MaxNumberUser { get;  set; }
        public ChatChannelMode ChannelMode { get;  set; }
        public DateTime ChannelLifeTime { get; set; }
        public DateTime ChannelCreatedTime { get; set; }
        public List<ChatSession> ChannelOperators { get; set; }
        public ChatSession ChannelCreator { get; set; }
        public List<Guid> BanList { get;  set; }
        public ConcurrentDictionary<Guid, ChatSession> ChannelUsers;
        public string Password { get; protected set; }

        public ChatChannelProperty(ChatSession creator)
        {
            MaxNumberUser = 200;
            Channelid = Guid.NewGuid();
            ChannelCreatedTime = DateTime.Now;
            ChannelMode = new ChatChannelMode();
            ChannelOperators = new List<ChatSession>();
            BanList = new List<Guid>();
            ChannelUsers = new ConcurrentDictionary<Guid, ChatSession>();
            ChannelCreator = creator;
        }
        public ChatChannelProperty(ChatSession creator,DateTime lifeTime)
        {
            MaxNumberUser = 200;
            Channelid = Guid.NewGuid();
            ChannelLifeTime = lifeTime;
            ChannelCreatedTime = DateTime.Now;
            ChannelMode = new ChatChannelMode();
            ChannelOperators = new List<ChatSession>();
            BanList = new List<Guid>();
            ChannelUsers = new ConcurrentDictionary<Guid, ChatSession>();
            ChannelCreator = creator;
        }
        public ChatChannelProperty(ChatSession creator, DateTime lifeTime,uint maxUser)
        {
            MaxNumberUser = maxUser;
            Channelid = Guid.NewGuid();
            ChannelLifeTime = lifeTime;
            ChannelCreatedTime = DateTime.Now;
            ChannelMode = new ChatChannelMode();
            ChannelOperators = new List<ChatSession>();
            BanList = new List<Guid>();
            ChannelUsers = new ConcurrentDictionary<Guid, ChatSession>();
            ChannelCreator = creator;
        }
        public bool SetProperties(ChatCommandBase cmd)
        {
            //password
          return  ChannelMode.SetModes(cmd);
        }
    }
}
