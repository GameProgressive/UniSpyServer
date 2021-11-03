using UniSpyServer.Servers.PresenceConnectionManager.Abstraction.BaseClass;
using UniSpyServer.Servers.PresenceConnectionManager.Entity.Contract;
using UniSpyServer.Servers.PresenceConnectionManager.Entity.Enumerate;
using UniSpyServer.Servers.PresenceConnectionManager.Entity.Structure.Misc;
using UniSpyServer.Servers.PresenceSearchPlayer.Entity.Exception.General;

namespace UniSpyServer.Servers.PresenceConnectionManager.Entity.Structure.Request
{
    /// <summary>
    /// Update a user's status information
    /// </summary>
    [RequestContract("status")]
    public sealed class StatusRequest : RequestBase
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


            if (!RequestKeyValues.ContainsKey("status"))
                throw new GPParseException("status is missing.");

            if (!RequestKeyValues.ContainsKey("statstring"))
                throw new GPParseException("statstring is missing.");

            if (!RequestKeyValues.ContainsKey("locstring"))
                throw new GPParseException("locstring is missing.");

            uint statusCode;
            if (!uint.TryParse(RequestKeyValues["status"], out statusCode))
            {
                throw new GPParseException("status format is incorrect.");
            }

            Status.CurrentStatus = (GPStatusCode)statusCode;
            Status.LocationString = RequestKeyValues["locstring"];
            Status.StatusString = RequestKeyValues["statstring"];
        }
    }
}
