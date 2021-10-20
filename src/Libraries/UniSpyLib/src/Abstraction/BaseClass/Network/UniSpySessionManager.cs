﻿using System.Collections.Concurrent;
using UniSpyServer.UniSpyLib.Abstraction.Interface;

namespace UniSpyServer.UniSpyLib.Abstraction.BaseClass.Network
{
    /// <summary>
    /// This class manages sessions that connect to this UniSpy distributed servers
    /// </summary>
    /// <typeparam name="TKey"></typeparam>
    /// <typeparam name="TSession"></typeparam>
    public abstract class UniSpySessionManager
    {
        /// <summary>
        /// The contravariance method for access
        /// </summary>
        public ConcurrentDictionary<object, IUniSpySession> SessionPool { get; private set; }
        public UniSpySessionManager()
        {
            SessionPool = new ConcurrentDictionary<object, IUniSpySession>();
        }

        public virtual bool Start()
        {
            return true;
        }
    }
}
