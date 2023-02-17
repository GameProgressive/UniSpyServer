using UniSpy.Server.PresenceConnectionManager.Abstraction.BaseClass;
using UniSpy.Server.PresenceSearchPlayer.Exception.General;

namespace UniSpy.Server.PresenceConnectionManager.Contract.Request
{

    public sealed class GetProfileRequest : RequestBase
    {
        public int ProfileId { get; private set; }
        public string SessionKey { get; private set; }
        public GetProfileRequest(string rawRequest) : base(rawRequest)
        {
        }

        public override void Parse()
        {
            base.Parse();

            if (!RequestKeyValues.ContainsKey("profileid"))
            {
                throw new GPParseException("profileid is missing");
            }

            int profileID;
            if (!int.TryParse(RequestKeyValues["profileid"], out profileID))
            {
                throw new GPParseException("profileid format is incorrect");
            }
            ProfileId = profileID;

            if (!RequestKeyValues.ContainsKey("sesskey"))
            {
                throw new GPParseException("sesskey is missing");
            }
            SessionKey = RequestKeyValues["sesskey"];
        }
    }
}
