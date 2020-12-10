using PresenceConnectionManager.Abstraction.BaseClass;
using PresenceSearchPlayer.Entity.Enumerate;
using System.Collections.Generic;

namespace PresenceConnectionManager.Entity.Structure.Request
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

            if (!KeyValues.ContainsKey("status") || !KeyValues.ContainsKey("statstring") || !KeyValues.ContainsKey("locstring"))
            {
                return GPErrorCode.Parse;
            }
            uint statusCode;
            if (!uint.TryParse(KeyValues["status"], out statusCode))
            {
                return GPErrorCode.Parse;
            }

            StatusCode = statusCode;
            LocationString = KeyValues["locstring"];
            StatusString = KeyValues["statstring"];

            return GPErrorCode.NoError;
        }
    }
}
