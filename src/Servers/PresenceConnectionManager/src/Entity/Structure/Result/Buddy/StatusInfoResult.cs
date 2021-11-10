using UniSpyServer.Servers.PresenceConnectionManager.Abstraction.BaseClass;
using UniSpyServer.Servers.PresenceConnectionManager.Entity.Structure.Misc;

namespace UniSpyServer.Servers.PresenceConnectionManager.Entity.Structure.Result.Buddy
{
    public sealed class StatusInfoResult : ResultBase
    {
        public uint ProfileID { get; set; }
        public uint ProductID { get; set; }
        public PCMUserStatusInfo StatusInfo { get; set; }
        public StatusInfoResult()
        {
        }
    }
}
