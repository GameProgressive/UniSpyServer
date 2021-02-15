using PresenceConnectionManager.Abstraction.BaseClass;
using PresenceConnectionManager.Entity.Enumerate;

namespace PresenceConnectionManager.Entity.Structure.Result
{
    internal sealed class StatusResult : PCMResultBase
    {
        public GPStatusCode UserStatus { get; set; }
        public string StatusString { get; set; }
        public string LocationString { get; set; }
        public StatusResult()
        {
        }
    }
}
