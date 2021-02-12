using System.Collections.Concurrent;
using System.Net;

namespace UniSpyLib.Abstraction.BaseClass
{
    public class UniSpySessionManagerBase<TSession>
    {
        public static ConcurrentDictionary<IPEndPoint, TSession> Sessions { get; protected set; }
        public UniSpySessionManagerBase()
        {
            Sessions = new ConcurrentDictionary<IPEndPoint, TSession>();
        }

        public static TSession GetSession(IPEndPoint endPoint)
        {
            TSession session;
            Sessions.TryGetValue(endPoint, out session);
            return session;
        }

        public static bool AddSession(IPEndPoint endPoint,TSession session)
        {
            return Sessions.TryAdd(endPoint, session);
        }

        public static bool DeleteSession(IPEndPoint endPoint)
        {
            return Sessions.TryRemove(endPoint, out _);
        }
    }
}
