using PresenceConnectionManager.Entity.BaseClass;
using PresenceSearchPlayer.Entity.Enumerator;
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

        public override GPError Parse()
        {
            var flag = base.Parse();
            if (flag != GPError.NoError)
            {
                return flag;
            }
            if (!_recv.ContainsKey("productid") || !_recv.ContainsKey("sesskey"))
            {
                return GPError.Parse;
            }

            if (!_recv.ContainsKey("sesskey"))
            {
                return GPError.Parse;
            }

            uint productID;
            if (!uint.TryParse(_recv["productid"], out productID))
            {
                return GPError.Parse;
            }

            ProductID = productID;

            uint profileID;
            if (!uint.TryParse(_recv["profileid"], out profileID))
            {
                return GPError.Parse;
            }
            ProfileID = profileID;

            return GPError.NoError;
        }
    }
}
