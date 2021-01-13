using PresenceConnectionManager.Abstraction.BaseClass;
using PresenceSearchPlayer.Entity.Enumerate;
using System.Collections.Generic;

namespace PresenceConnectionManager.Entity.Structure.Request
{
    internal class StatusRequest : PCMRequestBase
    {
        public StatusRequest(string rawRequest) : base(rawRequest)
        {
        }

        public uint StatusCode { get; private set; }
        public string StatusString { get; protected set; }
        public string LocationString { get; protected set; }

        public override void Parse()
        {
            base.Parse();
            if( ErrorCode != GPErrorCode.NoError)
            {
                return;
            }

            if (!KeyValues.ContainsKey("status") || !KeyValues.ContainsKey("statstring") || !KeyValues.ContainsKey("locstring"))
            {
                ErrorCode = GPErrorCode.Parse; return;
            }
            uint statusCode;
            if (!uint.TryParse(KeyValues["status"], out statusCode))
            {
                ErrorCode = GPErrorCode.Parse; return;
            }

            StatusCode = statusCode;
            LocationString = KeyValues["locstring"];
            StatusString = KeyValues["statstring"];
        }
    }
}
