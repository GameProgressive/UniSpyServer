using PresenceConnectionManager.Abstraction.BaseClass;
using PresenceConnectionManager.Entity.Enumerate;
using PresenceConnectionManager.Entity.Structure.Misc;
using PresenceSearchPlayer.Entity.Exception.General;


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
                throw new GPParseException("status is missing.");

            if (!KeyValues.ContainsKey("statstring"))
                throw new GPParseException("statstring is missing.");

            if (!KeyValues.ContainsKey("locstring"))
                throw new GPParseException("locstring is missing.");

            uint statusCode;
            if (!uint.TryParse(KeyValues["status"], out statusCode))
            {
                throw new GPParseException("status format is incorrect.");
            }

            Status.CurrentStatus = (GPStatusCode)statusCode;
            Status.LocationString = KeyValues["locstring"];
            Status.StatusString = KeyValues["statstring"];
        }
    }
}
