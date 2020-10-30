using PresenceConnectionManager.Entity.BaseClass;
using System;
using System.Collections.Generic;
using System.Text;

namespace PresenceConnectionManager.Entity.Structure.Request.General
{
    public class KeepAliveRequest : PCMRequestBase
    {
        public KeepAliveRequest(Dictionary<string, string> recv) : base(recv)
        {
        }
    }
}
