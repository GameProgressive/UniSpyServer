using PresenceConnectionManager.Abstraction.BaseClass;
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
                ErrorCode = GPErrorCode.Parse; return;
            }

            uint profileID;
            if (!uint.TryParse(KeyValues["profileid"], out profileID))
            {
                ErrorCode = GPErrorCode.Parse; return;
            }

            ProfileID = profileID;

        }
    }
}
