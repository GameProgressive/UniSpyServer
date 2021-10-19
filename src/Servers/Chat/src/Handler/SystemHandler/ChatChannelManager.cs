using Chat.Entity.Structure.Misc.ChannelInfo;
using System.Collections.Concurrent;

namespace Chat.Handler.SystemHandler.ChannelManage
{
    public sealed class ChatChannelManager
    {
        public static ConcurrentDictionary<string, Channel> Channels { get; private set; }
        static ChatChannelManager()
        {
            Channels = new ConcurrentDictionary<string, Channel>();
        }
        public void Start()
        {
            //start timer to check expired channel
        }
        public static bool GetChannel(string name, out Channel channel)
        {
            return Channels.TryGetValue(name, out channel);
        }
        public static bool AddChannel(string name, Channel channel)
        {
            return Channels.TryAdd(name, channel);
        }
        public static bool RemoveChannel(string name)
        {
            return Channels.TryRemove(name, out _);
        }
        public static bool RemoveChannel(Channel channel)
        {
            return RemoveChannel(channel.Property.ChannelName);
        }
    }
}
