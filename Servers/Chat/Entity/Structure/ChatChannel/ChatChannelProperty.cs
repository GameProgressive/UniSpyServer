using System;
namespace Chat.Entity.Structure.ChatChannel
{
    public class ChatChannelProperty
    {
        public uint MaxNumberUser { get; protected set; }
        public ChatChannelMode ChannelMode { get; protected set; }
        public DateTime ChannelLifeTime { get; protected set; }
        public DateTime ChannelCreatedTime { get; protected set; }

        public ChatChannelProperty(DateTime lifeTime)
        {
            ChannelLifeTime = lifeTime;
            ChannelCreatedTime = DateTime.Now;
        }
    }
}
