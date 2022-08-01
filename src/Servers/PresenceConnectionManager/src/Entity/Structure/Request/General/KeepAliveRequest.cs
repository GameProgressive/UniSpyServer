using UniSpyServer.Servers.PresenceConnectionManager.Abstraction.BaseClass;

namespace UniSpyServer.Servers.PresenceConnectionManager.Entity.Structure.Request
{

    public sealed class KeepAliveRequest : RequestBase
    {
        public KeepAliveRequest(string rawRequest) : base(rawRequest)
        {
        }
    }
}
