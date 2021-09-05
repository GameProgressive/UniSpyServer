using PresenceConnectionManager.Abstraction.BaseClass;
using PresenceConnectionManager.Entity.Structure.Misc;

namespace PresenceConnectionManager.Entity.Structure.Result
{
    internal sealed class StatusResult : PCMResultBase
    {
        public PCMUserStatus Status { get; set; }
        public StatusResult()
        {
        }
    }
}
