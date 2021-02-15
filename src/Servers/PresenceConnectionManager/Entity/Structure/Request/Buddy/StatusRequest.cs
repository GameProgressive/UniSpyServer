using PresenceConnectionManager.Abstraction.BaseClass;
using PresenceConnectionManager.Entity.Enumerate;
using PresenceSearchPlayer.Entity.Enumerate;

namespace PresenceConnectionManager.Entity.Structure.Request
{
    internal sealed class StatusRequest : PCMRequestBase
    {
        public GPStatusCode StatusCode { get; private set; }
        public string StatusString { get; private set; }
        public string LocationString { get; private set; }

        public StatusRequest(string rawRequest) : base(rawRequest)
        {
        }

        public override void Parse()
        {
            base.Parse();
            if (ErrorCode != GPErrorCode.NoError)
            {
                return;
            }

            if (!KeyValues.ContainsKey("status") || !KeyValues.ContainsKey("statstring") || !KeyValues.ContainsKey("locstring"))
            {
                ErrorCode = GPErrorCode.Parse;
                return;
            }
            uint statusCode;
            if (!uint.TryParse(KeyValues["status"], out statusCode))
            {
                ErrorCode = GPErrorCode.Parse;
                return;
            }

            StatusCode = (GPStatusCode)statusCode;
            LocationString = KeyValues["locstring"];
            StatusString = KeyValues["statstring"];
        }
    }
}
