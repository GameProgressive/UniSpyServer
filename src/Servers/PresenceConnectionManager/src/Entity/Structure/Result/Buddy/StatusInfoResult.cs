using UniSpy.Server.PresenceConnectionManager.Abstraction.BaseClass;
using UniSpy.Server.PresenceConnectionManager.Entity.Structure.Misc;

namespace UniSpy.Server.PresenceConnectionManager.Entity.Structure.Result
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
