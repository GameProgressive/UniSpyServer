using UniSpyServer.PresenceConnectionManager.Abstraction.BaseClass;
using UniSpyServer.PresenceConnectionManager.Entity.Structure.Misc;

namespace UniSpyServer.PresenceConnectionManager.Entity.Structure.Result
{
    public sealed class StatusResult : ResultBase
    {
        public PCMUserStatus Status { get; set; }
        public StatusResult()
        {
        }
    }
}
