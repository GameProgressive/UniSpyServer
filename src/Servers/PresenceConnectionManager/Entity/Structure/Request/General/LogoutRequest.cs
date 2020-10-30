using PresenceConnectionManager.Entity.BaseClass;
using System;
using System.Collections.Generic;
using System.Text;

namespace PresenceConnectionManager.Entity.Structure.Request.General
{
    public class LogoutRequest : PCMRequestBase
    {
        public LogoutRequest(Dictionary<string, string> recv) : base(recv)
        {
        }
    }
}
