using PresenceConnectionManager.Abstraction.BaseClass;
using UniSpyLib.Abstraction.BaseClass;

namespace PresenceConnectionManager.Entity.Structure.Request
{
    [Command("logout")]
    internal sealed class LogoutRequest : PCMRequestBase
    {
        public LogoutRequest(string rawRequest) : base(rawRequest)
        {
        }
    }
}
