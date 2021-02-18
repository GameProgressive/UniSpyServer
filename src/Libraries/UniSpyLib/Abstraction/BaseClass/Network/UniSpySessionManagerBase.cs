using System.Collections.Concurrent;
using UniSpyLib.Abstraction.Interface;

namespace UniSpyLib.Abstraction.BaseClass
{
    /// <summary>
    /// This class manages sessions that connect to this UniSpy distributed servers
    /// </summary>
    /// <typeparam name="TKey"></typeparam>
    /// <typeparam name="TSession"></typeparam>
    public abstract class UniSpySessionManagerBase
    {
        public ConcurrentDictionary<object, IUniSpySession> Sessions { get; protected set; }
        public UniSpySessionManagerBase()
        {
            Sessions = new ConcurrentDictionary<object, IUniSpySession>();
        }

        protected IUniSpySession GetSession(object key)
        {
            IUniSpySession session;
            if (Sessions.TryGetValue(key, out session))
            {
                return session;
            }
            else
            {
                return null;
            }
        }

        protected bool AddSession(object key, IUniSpySession session)
        {
            return Sessions.TryAdd(key, session);
        }

        protected bool DeleteSession(object key)
        {
            return Sessions.TryRemove(key, out _);
        }
    }
}
