using UniSpy.Server.PresenceConnectionManager.Abstraction.BaseClass;
using UniSpy.Server.PresenceConnectionManager.Aggregate.Misc;

namespace UniSpy.Server.PresenceConnectionManager.Contract.Result
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
