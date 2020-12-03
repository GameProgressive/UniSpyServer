using PresenceConnectionManager.Abstraction.BaseClass;
using PresenceSearchPlayer.Entity.Enumerate;
using System.Collections.Generic;

namespace PresenceConnectionManager.Entity.Structure.Request.Profile
{
    public class GetProfileRequest : PCMRequestBase
    {
        public uint ProfileID { get; protected set; }
        public GetProfileRequest(string rawRequest) : base(rawRequest)
        {
        }

        public override object Parse()
        {
            var flag = (GPErrorCode)base.Parse();
            if (flag != GPErrorCode.NoError)
            {
                return flag;
            }

            if (!_recv.ContainsKey("profileid"))
            {
                return GPErrorCode.Parse;
            }

            uint profileID;
            if (!uint.TryParse(_recv["profileid"], out profileID))
            {
                return GPErrorCode.Parse;
            }
            ProfileID = profileID;

            if (!_recv.ContainsKey("sesskey"))
            {
                return GPErrorCode.Parse;
            }



            return GPErrorCode.NoError;
        }
    }
}
