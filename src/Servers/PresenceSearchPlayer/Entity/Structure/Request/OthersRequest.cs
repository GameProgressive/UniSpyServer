using PresenceSearchPlayer.Abstraction.BaseClass;
using PresenceSearchPlayer.Entity.Enumerate;
using System.Collections.Generic;

namespace PresenceSearchPlayer.Entity.Structure.Request
{
    public class OthersRequest : PSPRequestBase
    {
        public OthersRequest(Dictionary<string, string> recv) : base(recv)
        {
        }

        public uint ProfileID { get; private set; }
        public string GameName { get; private set; }
        public uint NamespaceID { get; protected set; }
        public override object Parse()
        {
            var flag = (GPErrorCode)base.Parse();
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

            if (_recv.ContainsKey("namespaceid"))
            {
                uint namespaceID;
                if (!uint.TryParse(_recv["namespaceid"], out namespaceID))
                {
                    return GPErrorCode.Parse;
                }

                NamespaceID = namespaceID;
            }

            ProfileID = profileID;
            GameName = _recv["gamename"];
            return GPErrorCode.NoError;
        }
    }
}
