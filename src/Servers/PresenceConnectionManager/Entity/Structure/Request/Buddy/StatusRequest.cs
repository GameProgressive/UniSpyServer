using PresenceConnectionManager.Abstraction.BaseClass;
using PresenceConnectionManager.Entity.Enumerate;
using PresenceConnectionManager.Entity.Structure.Misc;
using PresenceSearchPlayer.Entity.Enumerate;

namespace PresenceConnectionManager.Entity.Structure.Request
{
    internal sealed class StatusRequest : PCMRequestBase
    {
        public PCMUserStatus Status { get; private set; }
        public bool IsGetStatus { get; set; }
        public StatusRequest(string rawRequest) : base(rawRequest)
        {
            Status = new PCMUserStatus();
            IsGetStatus = false;
        }

        public override void Parse()
        {
            base.Parse();


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

            Status.CurrentStatus = (GPStatusCode)statusCode;
            Status.LocationString = KeyValues["locstring"];
            Status.StatusString = KeyValues["statstring"];
        }
    }
}
