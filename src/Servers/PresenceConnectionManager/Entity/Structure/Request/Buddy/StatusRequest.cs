using PresenceConnectionManager.Entity.BaseClass;
using PresenceSearchPlayer.Entity.Enumerator;
using System.Collections.Generic;

namespace PresenceConnectionManager.Entity.Structure.Request.Buddy
{
    public class StatusRequest : PCMRequestBase
    {
        public StatusRequest(Dictionary<string, string> recv) : base(recv)
        {
        }

        public uint StatusCode { get; private set; }
        public string StatusString { get; protected set; }
        public string LocationString { get; protected set; }

        public override GPError Parse()
        {
            var flag = base.Parse();
            if (flag != GPError.NoError)
            {
                return flag;
            }

            if (!_recv.ContainsKey("status") || !_recv.ContainsKey("statstring") || !_recv.ContainsKey("locstring"))
            {
                return GPError.Parse;
            }
            uint statusCode;
            if (!uint.TryParse(_recv["status"], out statusCode))
            {
                return GPError.Parse;
            }

            StatusCode = statusCode;
            LocationString = _recv["locstring"];
            StatusString = _recv["statstring"];

            return GPError.NoError;
        }
    }
}
