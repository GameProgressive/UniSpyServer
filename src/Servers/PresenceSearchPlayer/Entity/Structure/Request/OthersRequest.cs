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
        public override GPErrorCode Parse()
        {
            var flag = base.Parse();
            if (flag != GPErrorCode.NoError)
            {
                return flag;
            }

            if (!_rawRequest.ContainsKey("gamename"))
            {
                return GPErrorCode.Parse;
            }

            if (!_rawRequest.ContainsKey("profileid") || !_rawRequest.ContainsKey("namespaceid"))
            {
                return GPErrorCode.Parse;
            }

            uint profileID = 0;
            if (!_rawRequest.ContainsKey("profileid") && !uint.TryParse(_rawRequest["profileid"], out profileID))
            {
                return GPErrorCode.Parse;

            }

            if (_rawRequest.ContainsKey("namespaceid"))
            {
                uint namespaceID;
                if (!uint.TryParse(_rawRequest["namespaceid"], out namespaceID))
                {
                    return GPErrorCode.Parse;
                }

                NamespaceID = namespaceID;
            }

            ProfileID = profileID;
            GameName = _rawRequest["gamename"];
            return GPErrorCode.NoError;
        }
    }
}
