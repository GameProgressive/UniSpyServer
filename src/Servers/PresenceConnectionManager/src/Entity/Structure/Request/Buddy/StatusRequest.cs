using UniSpy.Server.PresenceConnectionManager.Abstraction.BaseClass;
using UniSpy.Server.PresenceConnectionManager.Entity.Enumerate;
using UniSpy.Server.PresenceConnectionManager.Entity.Structure.Misc;
using UniSpy.Server.PresenceSearchPlayer.Entity.Exception.General;

namespace UniSpy.Server.PresenceConnectionManager.Entity.Structure.Request
{
    /// <summary>
    /// Update a user's status information
    /// </summary>

    public sealed class StatusRequest : RequestBase
    {
        public UserStatus Status { get; private set; }
        public bool IsGetStatus { get; set; }
        public StatusRequest(string rawRequest) : base(rawRequest)
        {
            Status = new UserStatus();
            IsGetStatus = false;
        }

        public override void Parse()
        {
            base.Parse();

            if (!RequestKeyValues.ContainsKey("status"))
                throw new GPParseException("status is missing.");

            if (!RequestKeyValues.ContainsKey("statstring"))
                throw new GPParseException("statstring is missing.");

            if (!RequestKeyValues.ContainsKey("locstring"))
                throw new GPParseException("locstring is missing.");

            int statusCode;
            if (!int.TryParse(RequestKeyValues["status"], out statusCode))
            {
                throw new GPParseException("status format is incorrect.");
            }

            Status.CurrentStatus = (GPStatusCode)statusCode;
            Status.LocationString = RequestKeyValues["locstring"];
            Status.StatusString = RequestKeyValues["statstring"];
        }
    }
}
