using Chat.Entity.Structure;
using Chat.Entity.Structure.ChatChannel;
using System;
using System.Collections.Concurrent;

namespace Chat.Handler.SystemHandler.ChannelManage
{
    public class ChatChannelManager
    {
        public static ConcurrentDictionary<string, ChatChannelBase> Channels;

        public ChatChannelManager()
        {
            Channels = new ConcurrentDictionary<string, ChatChannelBase>();
        }
        public void Start()
        {
            //start timer to check expired channel
        }
        public static bool GetChannel(string name, out ChatChannelBase channel)
        {
            return Channels.TryGetValue(name, out channel);
        }
        public static bool AddChannel(string name, ChatChannelBase channel)
        {
            return Channels.TryAdd(name, channel);
        }
    }
}
