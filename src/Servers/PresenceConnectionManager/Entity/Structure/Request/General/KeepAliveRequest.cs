using PresenceConnectionManager.Abstraction.BaseClass;
using System.Collections.Generic;

namespace PresenceConnectionManager.Entity.Structure.Request.General
{
    public class KeepAliveRequest : PCMRequestBase
    {
        public KeepAliveRequest(Dictionary<string, string> recv) : base(recv)
        {
        }
    }
}
