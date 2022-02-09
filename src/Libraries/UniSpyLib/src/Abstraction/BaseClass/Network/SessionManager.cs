using System.Collections.Concurrent;
using System.Collections.Generic;
using UniSpyServer.UniSpyLib.Abstraction.Interface;

namespace UniSpyServer.UniSpyLib.Abstraction.BaseClass.Network
{
    /// <summary>
    /// This class manages sessions that connect to this UniSpy distributed servers
    /// </summary>
    /// <typeparam name="TKey"></typeparam>
    /// <typeparam name="TSession"></typeparam>
    public abstract class SessionManager
    {
        /// <summary>
        /// The contravariance method for access
        /// </summary>
        public IDictionary<object, ISession> SessionPool { get; private set; }
        public SessionManager()
        {
            SessionPool = new ConcurrentDictionary<object, ISession>();
        }

        public virtual bool Start()
        {
            return true;
        }
    }
}
