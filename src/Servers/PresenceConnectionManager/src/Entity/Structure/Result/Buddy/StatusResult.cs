using UniSpyServer.Servers.PresenceConnectionManager.Abstraction.BaseClass;
using UniSpyServer.Servers.PresenceConnectionManager.Entity.Structure.Misc;

namespace UniSpyServer.Servers.PresenceConnectionManager.Entity.Structure.Result
{
    public sealed class StatusResult : ResultBase
    {
        public PCMUserStatus Status { get; set; }
        public StatusResult()
        {
        }
    }
}
