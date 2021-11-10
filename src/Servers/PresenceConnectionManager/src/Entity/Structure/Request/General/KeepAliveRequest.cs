using UniSpyServer.Servers.PresenceConnectionManager.Abstraction.BaseClass;
using UniSpyServer.Servers.PresenceConnectionManager.Entity.Contract;

namespace UniSpyServer.Servers.PresenceConnectionManager.Entity.Structure.Request
{
    [RequestContract("ka")]
    public sealed class KeepAliveRequest : RequestBase
    {
        public KeepAliveRequest(string rawRequest) : base(rawRequest)
        {
        }
    }
}
