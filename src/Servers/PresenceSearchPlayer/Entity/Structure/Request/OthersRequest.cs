using PresenceSearchPlayer.Abstraction.BaseClass;
using PresenceSearchPlayer.Entity.Enumerate;
using System.Collections.Generic;

namespace PresenceSearchPlayer.Entity.Structure.Request
{
    internal class OthersRequest : PSPRequestBase
    {
        public OthersRequest(string rawRequest) :base(rawRequest)
        {
        }

        public uint ProfileID { get; private set; }
        public string GameName { get; private set; }
        public uint NamespaceID { get; protected set; }
        public override void Parse()
        {
            base.Parse();
            if (ErrorCode != GPErrorCode.NoError)
            {
                return;
            }

            if (!RequestKeyValues.ContainsKey("gamename"))
            {
                ErrorCode = GPErrorCode.Parse;
                return;
            }

            if (!RequestKeyValues.ContainsKey("profileid") || !RequestKeyValues.ContainsKey("namespaceid"))
            {
                ErrorCode = GPErrorCode.Parse;
                return;
            }

            uint profileID = 0;
            if (!RequestKeyValues.ContainsKey("profileid") && !uint.TryParse(RequestKeyValues["profileid"], out profileID))
            {
                ErrorCode = GPErrorCode.Parse;
                return;
            }

            if (RequestKeyValues.ContainsKey("namespaceid"))
            {
                uint namespaceID;
                if (!uint.TryParse(RequestKeyValues["namespaceid"], out namespaceID))
                {
                    ErrorCode = GPErrorCode.Parse;
                    return;
                }

                NamespaceID = namespaceID;
            }

            ProfileID = profileID;
            GameName = RequestKeyValues["gamename"];
            ErrorCode = GPErrorCode.NoError;
        }
    }
}
