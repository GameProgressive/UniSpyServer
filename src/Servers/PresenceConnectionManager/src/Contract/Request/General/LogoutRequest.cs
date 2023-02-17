using UniSpy.Server.PresenceConnectionManager.Abstraction.BaseClass;

namespace UniSpy.Server.PresenceConnectionManager.Contract.Request
{

    public sealed class LogoutRequest : RequestBase
    {
        public LogoutRequest(string rawRequest) : base(rawRequest)
        {
        }
    }
}
