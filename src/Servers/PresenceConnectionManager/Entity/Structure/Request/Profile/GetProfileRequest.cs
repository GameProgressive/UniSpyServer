using PresenceConnectionManager.Abstraction.BaseClass;
using PresenceSearchPlayer.Abstraction.BaseClass;
using PresenceSearchPlayer.Entity.Enumerate;

namespace PresenceConnectionManager.Entity.Structure.Request.Profile
{
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
                throw new GPGeneralException("profileid is missing", GPErrorCode.Parse);
            }

            uint profileID;
            if (!uint.TryParse(KeyValues["profileid"], out profileID))
            {
                throw new GPGeneralException("profileid format is incorrect", GPErrorCode.Parse);
            }
            ProfileID = profileID;

            if (!KeyValues.ContainsKey("sesskey"))
            {
                throw new GPGeneralException("sesskey is missing", GPErrorCode.Parse);
            }
            SessionKey = KeyValues["sesskey"];
        }
    }
}
