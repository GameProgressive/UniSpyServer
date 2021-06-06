using PresenceConnectionManager.Abstraction.BaseClass;
using PresenceConnectionManager.Entity.Enumerate;
using PresenceConnectionManager.Entity.Structure.Misc;
using PresenceSearchPlayer.Abstraction.BaseClass;
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


            if (!KeyValues.ContainsKey("status"))
                throw new GPGeneralException("status is missing.", GPErrorCode.Parse);

            if (!KeyValues.ContainsKey("statstring"))
                throw new GPGeneralException("statstring is missing.", GPErrorCode.Parse);

            if (!KeyValues.ContainsKey("locstring"))
                throw new GPGeneralException("locstring is missing.", GPErrorCode.Parse);

            uint statusCode;
            if (!uint.TryParse(KeyValues["status"], out statusCode))
            {
                throw new GPGeneralException("status format is incorrect.", GPErrorCode.Parse);
            }

            Status.CurrentStatus = (GPStatusCode)statusCode;
            Status.LocationString = KeyValues["locstring"];
            Status.StatusString = KeyValues["statstring"];
        }
    }
}
