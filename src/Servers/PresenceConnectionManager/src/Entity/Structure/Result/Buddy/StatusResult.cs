using PresenceConnectionManager.Abstraction.BaseClass;
using PresenceConnectionManager.Entity.Structure.Misc;

namespace PresenceConnectionManager.Entity.Structure.Result
{
    public sealed class StatusResult : ResultBase
    {
        public PCMUserStatus Status { get; set; }
        public StatusResult()
        {
        }
    }
}
