using PresenceConnectionManager.Abstraction.BaseClass;
using PresenceSearchPlayer.Abstraction.BaseClass;
using PresenceSearchPlayer.Entity.Enumerate;

namespace PresenceConnectionManager.Entity.Structure.Request
{
    internal class InviteToRequest : PCMRequestBase
    {
        public uint ProductID { get; protected set; }
        public uint ProfileID { get; protected set; }
        public InviteToRequest(string rawRequest) : base(rawRequest)
        {
        }

        public override void Parse()
        {
            base.Parse();

            if (!KeyValues.ContainsKey("productid"))
            {
                throw new GPGeneralException("productid is missing.", GPErrorCode.Parse);
            }

            if (!KeyValues.ContainsKey("sesskey"))
            {
                throw new GPGeneralException("sesskey is missing.", GPErrorCode.Parse);

            }

            uint productID;
            if (!uint.TryParse(KeyValues["productid"], out productID))
            {
                throw new GPGeneralException("productid format is incorrect.", GPErrorCode.Parse);
            }

            ProductID = productID;

            uint profileID;
            if (!uint.TryParse(KeyValues["profileid"], out profileID))
            {
                throw new GPGeneralException("profileid format is incorrect.", GPErrorCode.Parse);
            }
            ProfileID = profileID;
        }
    }
}
