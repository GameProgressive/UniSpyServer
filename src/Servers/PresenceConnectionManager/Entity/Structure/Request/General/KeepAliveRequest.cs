using PresenceConnectionManager.Abstraction.BaseClass;
using System.Collections.Generic;

namespace  PresenceConnectionManager.Entity.Structure.Request
{
    public class KeepAliveRequest : PCMRequestBase
    {
        public KeepAliveRequest(string rawRequest) : base(rawRequest)
        {
        }
    }
}
