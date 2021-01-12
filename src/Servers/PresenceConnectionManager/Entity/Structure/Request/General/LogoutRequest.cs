using PresenceConnectionManager.Abstraction.BaseClass;
using System.Collections.Generic;

namespace  PresenceConnectionManager.Entity.Structure.Request
{
    internal class LogoutRequest : PCMRequestBase
    {
        public LogoutRequest(string rawRequest) : base(rawRequest)
        {
        }
    }
}
