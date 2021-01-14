using PresenceConnectionManager.Abstraction.BaseClass;
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
            if (ErrorCode != GPErrorCode.NoError)
            {
                return;
            }
            if (!KeyValues.ContainsKey("productid") || !KeyValues.ContainsKey("sesskey"))
            {
                ErrorCode = GPErrorCode.Parse; return;
            }

            if (!KeyValues.ContainsKey("sesskey"))
            {
                ErrorCode = GPErrorCode.Parse; return;
            }

            uint productID;
            if (!uint.TryParse(KeyValues["productid"], out productID))
            {
                ErrorCode = GPErrorCode.Parse; return;
            }

            ProductID = productID;

            uint profileID;
            if (!uint.TryParse(KeyValues["profileid"], out profileID))
            {
                ErrorCode = GPErrorCode.Parse; return;
            }
            ProfileID = profileID;
        }
    }
}
