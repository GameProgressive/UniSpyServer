using System;
using System.Collections.Concurrent;
using System.Net;
using NatNegotiation.Server;

namespace NatNegotiation.Handler.SystemHandler.NegotiationSession
{
    public class NatNegotiatorPool
    {
        public static ConcurrentDictionary<string, NatNegSession> Sessions;

        public NatNegotiatorPool()
        {
            Sessions = new ConcurrentDictionary<string, NatNegSession>();
        }

        public void Start()
        { }
    }
}
