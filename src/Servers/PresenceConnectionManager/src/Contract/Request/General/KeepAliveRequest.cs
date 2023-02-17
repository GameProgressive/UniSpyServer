using UniSpy.Server.PresenceConnectionManager.Abstraction.BaseClass;

namespace UniSpy.Server.PresenceConnectionManager.Contract.Request
{

    public sealed class KeepAliveRequest : RequestBase
    {
        public KeepAliveRequest(string rawRequest) : base(rawRequest)
        {
        }
    }
}
