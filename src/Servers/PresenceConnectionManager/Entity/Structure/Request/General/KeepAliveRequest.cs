using PresenceConnectionManager.Abstraction.BaseClass;

namespace PresenceConnectionManager.Entity.Structure.Request
{
    internal sealed class KeepAliveRequest : PCMRequestBase
    {
        public KeepAliveRequest(string rawRequest) : base(rawRequest)
        {
        }
    }
}
