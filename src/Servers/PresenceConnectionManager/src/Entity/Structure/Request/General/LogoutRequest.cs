using UniSpyServer.PresenceConnectionManager.Abstraction.BaseClass;
using UniSpyServer.PresenceConnectionManager.Entity.Contract;

namespace UniSpyServer.PresenceConnectionManager.Entity.Structure.Request
{
    [RequestContract("logout")]
    public sealed class LogoutRequest : RequestBase
    {
        public LogoutRequest(string rawRequest) : base(rawRequest)
        {
        }
    }
}
