using UniSpy.Server.PresenceConnectionManager.Abstraction.BaseClass;
using UniSpy.Server.PresenceConnectionManager.Entity.Structure.Misc;

namespace UniSpy.Server.PresenceConnectionManager.Entity.Structure.Result
{
    public sealed class StatusResult : ResultBase
    {
        public UserStatus Status { get; set; }
        public StatusResult()
        {
        }
    }
}
