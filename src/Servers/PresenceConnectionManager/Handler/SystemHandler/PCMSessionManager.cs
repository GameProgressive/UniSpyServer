using System;
using PresenceConnectionManager.Network;
using UniSpyLib.Abstraction.BaseClass;

namespace PresenceConnectionManager.Handler.SystemHandler
{
    internal sealed class PCMSessionManager : UniSpySessionManagerBase<Guid, PCMSession>
    {
        public PCMSessionManager()
        {
        }
    }
}
