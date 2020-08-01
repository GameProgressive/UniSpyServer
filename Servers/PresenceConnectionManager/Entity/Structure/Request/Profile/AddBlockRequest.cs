using System;
using System.Collections.Generic;
using PresenceConnectionManager.Entity.BaseClass;
using PresenceSearchPlayer.Entity.Enumerator;

namespace PresenceConnectionManager.Entity.Structure.Request.Buddy
{
    public class AddBlockRequest : PCMRequestBase
    {
        public uint ProfileID;
        public AddBlockRequest(Dictionary<string, string> recv) : base(recv)
        {
        }

        public override GPError  Parse()
        {
            var flag = base.Parse();
            if (flag != GPError.NoError)
            {
                return flag;
            }

            if (!_recv.ContainsKey("profileid"))
            {
                return GPError.Parse;
            }

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
