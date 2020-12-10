using PresenceConnectionManager.Abstraction.BaseClass;
using PresenceSearchPlayer.Entity.Enumerate;
using System.Collections.Generic;

namespace PresenceConnectionManager.Entity.Structure.Request
{
    public class AddBlockRequest : PCMRequestBase
    {
        public uint ProfileID;
        public AddBlockRequest(string rawRequest) : base(rawRequest)
        {
        }

        public override object Parse()
        {
            var flag = (GPErrorCode)base.Parse();

            if (flag != GPErrorCode.NoError)
            {
                return flag;
            }

            if (!KeyValues.ContainsKey("profileid"))
            {
                return GPErrorCode.Parse;
            }

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
