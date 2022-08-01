using UniSpyServer.Servers.PresenceConnectionManager.Abstraction.BaseClass;
using UniSpyServer.Servers.PresenceConnectionManager.Entity.Structure.Misc;

namespace UniSpyServer.Servers.PresenceConnectionManager.Entity.Structure.Result
{
    public sealed class StatusInfoResult : ResultBase
    {
        public int ProfileId { get; set; }
        public int ProductId { get; set; }
        public UserStatusInfo StatusInfo { get; set; }
        public StatusInfoResult()
        {
        }
    }
}
