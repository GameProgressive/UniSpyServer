using System;
using System.Collections.Concurrent;
using Chat.Entity.Structure;

namespace Chat.Handler.SystemHandler.ChannelManage
{
    public class ChatChannelManager
    {
        public static ConcurrentDictionary<Guid, ChatChannel> Channels;

        public ChatChannelManager()
        {
            Channels = new ConcurrentDictionary<Guid, ChatChannel>();
        }
        public void Start()
        { }
    }
}
