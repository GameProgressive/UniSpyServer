using System;
using System.Collections.Concurrent;
using Chat.Server;

namespace Chat.Handler.SystemHandler.ChatSessionManage
{
    public class ChatSessionManager
    {
        public static ConcurrentDictionary<Guid, ChatSession> Sessions;

        public ChatSessionManager()
        {
            Sessions = new ConcurrentDictionary<Guid, ChatSession>();
        }

        public void Start()
        { }
    }
}
