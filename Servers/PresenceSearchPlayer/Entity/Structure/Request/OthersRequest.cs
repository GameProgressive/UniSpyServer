using System.Collections.Generic;
using PresenceSearchPlayer.Entity.Enumerator;
using PresenceSearchPlayer.Entity.Structure.Model;

namespace PresenceSearchPlayer.Entity.Structure.Request
{
    public class OthersRequest : PSPRequestBase
    {
        public OthersRequest(Dictionary<string, string> recv) : base(recv)
        {
        }

        public uint ProfileID { get; private set; }
        public string GameName { get; private set; }

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
