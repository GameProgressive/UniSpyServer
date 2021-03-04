using System.Collections.Concurrent;
using System.Linq;
using System.Net;
using UniSpyLib.Abstraction.Interface;
using UniSpyLib.Network;

namespace UniSpyLib.Abstraction.BaseClass
{
    /// <summary>
    /// This class manages sessions that connect to this UniSpy distributed servers
    /// </summary>
    /// <typeparam name="TKey"></typeparam>
    /// <typeparam name="TSession"></typeparam>
    public abstract class UniSpySessionManagerBase
    {
        /// <summary>
        /// The contravariance method for access
        /// </summary>
        public ConcurrentDictionary<object, IUniSpySession> Sessions { get; private set; }
            
        public UniSpySessionManagerBase()
        {
            Sessions = new ConcurrentDictionary<object, IUniSpySession>();
        }
    }
}
