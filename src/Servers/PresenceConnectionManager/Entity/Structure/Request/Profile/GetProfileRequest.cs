using PresenceConnectionManager.Abstraction.BaseClass;
using PresenceSearchPlayer.Entity.Exception.General;


namespace PresenceConnectionManager.Entity.Structure.Request.Profile
{
    [Command("getprofile")]
    internal sealed class GetProfileRequest : PCMRequestBase
    {
        public uint ProfileID { get; private set; }
        public string SessionKey { get; private set; }
        public GetProfileRequest(string rawRequest) : base(rawRequest)
        {
        }

        public override void Parse()
        {
            base.Parse();


            if (!KeyValues.ContainsKey("profileid"))
            {
                throw new GPParseException("profileid is missing");
            }

            uint profileID;
            if (!uint.TryParse(KeyValues["profileid"], out profileID))
            {
                throw new GPParseException("profileid format is incorrect");
            }
            ProfileID = profileID;

            if (!KeyValues.ContainsKey("sesskey"))
            {
                throw new GPParseException("sesskey is missing");
            }
            SessionKey = KeyValues["sesskey"];
        }
    }
}
