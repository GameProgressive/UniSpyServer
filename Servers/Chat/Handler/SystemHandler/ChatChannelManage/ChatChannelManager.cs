using Chat.Entity.Structure;
using Chat.Entity.Structure.ChatChannel;
using System;
using System.Collections.Concurrent;

namespace Chat.Handler.SystemHandler.ChannelManage
{
    public class ChatChannelManager
    {
        public static ConcurrentDictionary<Guid, ChatChannelBase> Channels;

        public ChatChannelManager()
        {
            Channels = new ConcurrentDictionary<Guid, ChatChannelBase>();
        }
        public void Start()
        { }
    }
}
