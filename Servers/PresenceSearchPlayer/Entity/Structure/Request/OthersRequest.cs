using System;
using System.Collections.Generic;
using PresenceSearchPlayer.Entity.Structure.Model;
using PresenceSearchPlayer.Enumerator;

namespace PresenceSearchPlayer.Entity.Structure.Request
{
    public class OthersRequest : PSPRequestModelBase
    {
        public OthersRequest(Dictionary<string, string> recv) : base(recv)
        {
        }

        public override GPErrorCode Parse()
        {
            var flag = base.Parse();
            if (flag != GPErrorCode.NoError)
            {
                return flag;
            }

            if (!_recv.ContainsKey("gamename"))
            {
                return GPErrorCode.Parse;
            }

            if (!_recv.ContainsKey("profileid") || !_recv.ContainsKey("namespaceid"))
            {
                return GPErrorCode.Parse;
            }

            uint profileID = 0;
            if (!_recv.ContainsKey("profileid") && !uint.TryParse(_recv["profileid"], out profileID))
            {
                return GPErrorCode.Parse;
            }

            ProfileID = profileID;
            GameName = _recv["gamename"];
            return GPErrorCode.NoError;
        }
    }
}
