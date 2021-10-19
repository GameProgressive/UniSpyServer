using PresenceConnectionManager.Abstraction.BaseClass;
using PresenceConnectionManager.Entity.Structure.Misc;

namespace PresenceConnectionManager.Entity.Structure.Result.Buddy
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
