using PresenceConnectionManager.Abstraction.BaseClass;
using PresenceSearchPlayer.Entity.Enumerate;
using System.Collections.Generic;

namespace PresenceConnectionManager.Entity.Structure.Request.Buddy
{
    public class StatusRequest : PCMRequestBase
    {
        public StatusRequest(string rawRequest) : base(rawRequest)
        {
        }

        public uint StatusCode { get; private set; }
        public string StatusString { get; protected set; }
        public string LocationString { get; protected set; }

        public override object Parse()
        {
            var flag = (GPErrorCode)base.Parse();
            if (flag != GPErrorCode.NoError)
            {
                return flag;
            }

            if (!_recv.ContainsKey("status") || !_recv.ContainsKey("statstring") || !_recv.ContainsKey("locstring"))
            {
                return GPErrorCode.Parse;
            }
            uint statusCode;
            if (!uint.TryParse(_recv["status"], out statusCode))
            {
                return GPErrorCode.Parse;
            }

            StatusCode = statusCode;
            LocationString = _recv["locstring"];
            StatusString = _recv["statstring"];

            return GPErrorCode.NoError;
        }
    }
}
