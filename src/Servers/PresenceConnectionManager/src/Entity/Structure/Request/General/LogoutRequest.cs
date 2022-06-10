using UniSpyServer.Servers.PresenceConnectionManager.Abstraction.BaseClass;

namespace UniSpyServer.Servers.PresenceConnectionManager.Entity.Structure.Request
{

    public sealed class LogoutRequest : RequestBase
    {
        public LogoutRequest(string rawRequest) : base(rawRequest)
        {
        }
    }
}
