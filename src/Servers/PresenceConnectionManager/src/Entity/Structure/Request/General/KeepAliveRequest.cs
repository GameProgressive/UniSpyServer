using UniSpyServer.PresenceConnectionManager.Abstraction.BaseClass;
using UniSpyServer.PresenceConnectionManager.Entity.Contract;

namespace UniSpyServer.PresenceConnectionManager.Entity.Structure.Request
{
    [RequestContract("ka")]
    public sealed class KeepAliveRequest : RequestBase
    {
        public KeepAliveRequest(string rawRequest) : base(rawRequest)
        {
        }
    }
}
