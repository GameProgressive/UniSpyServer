using System;
using System.Collections.Generic;
using PresenceConnectionManager.Entity.BaseClass;
using PresenceSearchPlayer.Entity.Enumerator;

namespace PresenceConnectionManager.Entity.Structure.Request.Profile
{
    public class GetProfileRequest : PCMRequestBase
    {
        public uint ProfileID { get; protected set; }
        public GetProfileRequest(Dictionary<string, string> recv) : base(recv)
        {
        }

        public override GPErrorCode Parse()
        {
            var flag = base.Parse();
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
