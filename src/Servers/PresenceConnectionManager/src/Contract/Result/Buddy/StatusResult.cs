using UniSpy.Server.PresenceConnectionManager.Abstraction.BaseClass;
using UniSpy.Server.PresenceConnectionManager.Aggregate.Misc;

namespace UniSpy.Server.PresenceConnectionManager.Contract.Result
{
    public sealed class StatusResult : ResultBase
    {
        public UserStatus Status { get; set; }
        public StatusResult()
        {
        }
    }
}
