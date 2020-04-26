using Chat.Server;
using System;
using System.Collections.Concurrent;
using System.Linq;

namespace Chat.Handler.SystemHandler.ChatSessionManage
{
    public class ChatSessionManager
    {
        public static ConcurrentDictionary<Guid, ChatSession> Sessions { get; protected set; }

        public ChatSessionManager()
        {
            Sessions = new ConcurrentDictionary<Guid, ChatSession>();
        }

        public void Start()
        { }

        public static bool AddSession(ChatSession session)
        {
            return Sessions.TryAdd(session.Id, session);
        }
        public static bool RemoveSession(ChatSession session)
        {
            return Sessions.TryRemove(session.Id, out _);
        }
        public static bool IsNickNameExisted(string nickname)
        {
            if (Sessions.Where(s => s.Value.UserInfo.NickName == nickname).Count() != 0)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
    }
}
