using PresenceSearchPlayer.Abstraction.BaseClass;
using PresenceSearchPlayer.Entity.Enumerate;
using System.Collections.Generic;

namespace PresenceSearchPlayer.Entity.Structure.Request
{
    public class OthersRequest : PSPRequestBase
    {
        public OthersRequest(string rawRequest) :base(rawRequest)
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

            if (!RequestKeyValues.ContainsKey("gamename"))
            {
                return GPErrorCode.Parse;
            }

            if (!RequestKeyValues.ContainsKey("profileid") || !RequestKeyValues.ContainsKey("namespaceid"))
            {
                return GPErrorCode.Parse;
            }

            uint profileID = 0;
            if (!RequestKeyValues.ContainsKey("profileid") && !uint.TryParse(RequestKeyValues["profileid"], out profileID))
            {
                return GPErrorCode.Parse;

            }

            if (RequestKeyValues.ContainsKey("namespaceid"))
            {
                uint namespaceID;
                if (!uint.TryParse(RequestKeyValues["namespaceid"], out namespaceID))
                {
                    return GPErrorCode.Parse;
                }

                NamespaceID = namespaceID;
            }

            ProfileID = profileID;
            GameName = RequestKeyValues["gamename"];
            return GPErrorCode.NoError;
        }
    }
}
