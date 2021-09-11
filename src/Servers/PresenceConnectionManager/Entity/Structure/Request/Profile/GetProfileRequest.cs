using PresenceConnectionManager.Abstraction.BaseClass;
using PresenceConnectionManager.Entity.Contract;
using PresenceSearchPlayer.Entity.Exception.General;
using UniSpyLib.Abstraction.BaseClass;

namespace PresenceConnectionManager.Entity.Structure.Request.Profile
{
    [RequestContract("getprofile")]
    internal sealed class GetProfileRequest : RequestBase
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
