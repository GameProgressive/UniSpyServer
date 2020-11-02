using PresenceConnectionManager.Abstraction.BaseClass;
using System.Collections.Generic;

namespace PresenceConnectionManager.Entity.Structure.Request.General
{
    public class LogoutRequest : PCMRequestBase
    {
        public LogoutRequest(Dictionary<string, string> recv) : base(recv)
        {
        }
    }
}
