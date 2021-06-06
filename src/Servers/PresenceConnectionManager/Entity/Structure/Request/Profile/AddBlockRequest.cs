using PresenceConnectionManager.Abstraction.BaseClass;
using PresenceSearchPlayer.Abstraction.BaseClass;
using PresenceSearchPlayer.Entity.Enumerate;

namespace PresenceConnectionManager.Entity.Structure.Request
{
    internal sealed class AddBlockRequest : PCMRequestBase
    {
        public uint ProfileID;
        public AddBlockRequest(string rawRequest) : base(rawRequest)
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

        }
    }
}
