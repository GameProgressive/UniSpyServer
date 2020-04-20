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
        { }
    }
}
