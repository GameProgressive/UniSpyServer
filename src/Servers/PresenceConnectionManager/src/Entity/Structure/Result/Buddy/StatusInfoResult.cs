using UniSpyServer.PresenceConnectionManager.Abstraction.BaseClass;
using UniSpyServer.PresenceConnectionManager.Entity.Structure.Misc;

namespace UniSpyServer.PresenceConnectionManager.Entity.Structure.Result.Buddy
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
