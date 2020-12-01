using PresenceConnectionManager.Abstraction.BaseClass;
using PresenceSearchPlayer.Entity.Enumerate;
using System.Collections.Generic;

namespace PresenceConnectionManager.Entity.Structure.Request.Buddy
{
    public class InviteToRequest : PCMRequestBase
    {
        public uint ProductID { get; protected set; }
        public uint ProfileID { get; protected set; }
        public InviteToRequest(Dictionary<string, string> recv) : base(recv)
        {
        }

        public override GPErrorCode Parse()
        {
            var flag = base.Parse();
            if (flag != GPErrorCode.NoError)
            {
                return flag;
            }
            if (!_recv.ContainsKey("productid") || !_recv.ContainsKey("sesskey"))
            {
                return GPErrorCode.Parse;
            }

            if (!_recv.ContainsKey("sesskey"))
            {
                return GPErrorCode.Parse;
            }

            uint productID;
            if (!uint.TryParse(_recv["productid"], out productID))
            {
                return GPErrorCode.Parse;
            }

            ProductID = productID;

            uint profileID;
            if (!uint.TryParse(_recv["profileid"], out profileID))
            {
                return GPErrorCode.Parse;
            }
            ProfileID = profileID;

            return GPErrorCode.NoError;
        }
    }
}
