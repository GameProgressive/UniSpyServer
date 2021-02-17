using System.Collections.Concurrent;

namespace UniSpyLib.Abstraction.BaseClass
{
    public class UniSpySessionManagerBase<TKey, TSession>
    {
        public static ConcurrentDictionary<TKey, TSession> Sessions { get; protected set; }
        static UniSpySessionManagerBase()
        {
            Sessions = new ConcurrentDictionary<TKey, TSession>();
        }

        public static TSession GetSession(TKey key)
        {
            TSession session;
            if (Sessions.TryGetValue(key, out session))
            {
                return session;
            }
            else
            {
                return default(TSession);
            }
        }

        public static bool AddSession(TKey key, TSession session)
        {
            return Sessions.TryAdd(key, session);
        }

        public static bool DeleteSession(TKey key)
        {
            return Sessions.TryRemove(key, out _);
        }
    }
}
