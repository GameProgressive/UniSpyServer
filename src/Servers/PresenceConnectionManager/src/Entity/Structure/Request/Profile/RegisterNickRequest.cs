using UniSpy.Server.PresenceConnectionManager.Abstraction.BaseClass;
using UniSpy.Server.PresenceSearchPlayer.Entity.Exception.General;

namespace UniSpy.Server.PresenceConnectionManager.Entity.Structure.Request
{

    public sealed class RegisterNickRequest : RequestBase
    {
        public string UniqueNick { get; private set; }
        public string SessionKey { get; private set; }
        public int PartnerId { get; private set; }
        public RegisterNickRequest(string rawRequest) : base(rawRequest)
        {
        }

        public override void Parse()
        {
            base.Parse();

            if (!RequestKeyValues.ContainsKey("sesskey"))
            {
                throw new GPParseException("sesskey is missing");
            }
            SessionKey = RequestKeyValues["sesskey"];

            if (!RequestKeyValues.ContainsKey("uniquenick"))
            {
                throw new GPParseException("uniquenick is missing");
            }
            UniqueNick = RequestKeyValues["uniquenick"];

            // PartnerId is optional
            if (RequestKeyValues.ContainsKey("partnerid"))
            {
                int partnerID;
                if (!int.TryParse(RequestKeyValues["partnerid"], out partnerID))
                {
                    throw new GPParseException("partnerid is missing");
                }
                PartnerId = partnerID;
            }
        }
    }
}
