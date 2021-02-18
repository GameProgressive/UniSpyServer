using System;
using UniSpyLib.Abstraction.Interface;
using UniSpyLib.Network;

namespace UniSpyLib.Abstraction.BaseClass.Network.TCP
{
    public class UniSpyTCPSessionManagerBase : UniSpySessionManagerBase
    {
        public UniSpyTCPSessionManagerBase()
        {
        }

        public UniSpyTCPSessionBase GetSession(Guid key)
        {
            IUniSpySession session;
            if (Sessions.TryGetValue(key, out session))
            {
                return (UniSpyTCPSessionBase)session;
            }
            else
            {
                return null;
            }
        }

        public UniSpyTCPSessionBase GetOrAddSession(Guid key, UniSpyTCPSessionBase session)
        {
            return (UniSpyTCPSessionBase)Sessions.GetOrAdd(key, session);
        }

        public bool AddSession(Guid key, UniSpyTCPSessionBase session)
        {
            return Sessions.TryAdd(key, session);
        }

        public bool DeleteSession(Guid key)
        {
            return Sessions.TryRemove(key, out _);
        }
    }
}
