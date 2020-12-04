using PresenceConnectionManager.Abstraction.BaseClass;
using PresenceSearchPlayer.Entity.Enumerate;
using System.Collections.Generic;

namespace PresenceConnectionManager.Entity.Structure.Request.Buddy
{
    public class InviteToRequest : PCMRequestBase
    {
        public uint ProductID { get; protected set; }
        public uint ProfileID { get; protected set; }
        public InviteToRequest(string rawRequest) : base(rawRequest)
        {
        }

        public override object Parse()
        {
            var flag = (GPErrorCode)base.Parse();
            if (flag != GPErrorCode.NoError)
            {
                return flag;
            }
            if (!KeyValues.ContainsKey("productid") || !KeyValues.ContainsKey("sesskey"))
            {
                return GPErrorCode.Parse;
            }

            if (!KeyValues.ContainsKey("sesskey"))
            {
                return GPErrorCode.Parse;
            }

            uint productID;
            if (!uint.TryParse(KeyValues["productid"], out productID))
            {
                return GPErrorCode.Parse;
            }

            ProductID = productID;

            uint profileID;
            if (!uint.TryParse(KeyValues["profileid"], out profileID))
            {
                return GPErrorCode.Parse;
            }
            ProfileID = profileID;

            return GPErrorCode.NoError;
        }
    }
}
