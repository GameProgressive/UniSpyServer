using Chat.Entity.Structure.Misc.ChannelInfo;
using System.Collections.Concurrent;

namespace Chat.Handler.SystemHandler.ChannelManage
{
    public class ChatChannelManager
    {
        public static ConcurrentDictionary<string, ChatChannel> Channels;

        public ChatChannelManager()
        {
            Channels = new ConcurrentDictionary<string, ChatChannel>();
        }
        public void Start()
        {
            //start timer to check expired channel
        }
        public static bool GetChannel(string name, out ChatChannel channel)
        {
            return Channels.TryGetValue(name, out channel);
        }
        public static bool AddChannel(string name, ChatChannel channel)
        {
            return Channels.TryAdd(name, channel);
        }
        public static bool RemoveChannel(string name)
        {
            return Channels.TryRemove(name, out _);
        }
        public static bool RemoveChannel(ChatChannel channel)
        {
            return RemoveChannel(channel.Property.ChannelName);
        }
    }
}
