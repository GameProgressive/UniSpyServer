using PresenceSearchPlayer.Abstraction.BaseClass;
using PresenceSearchPlayer.Entity.Enumerate;
using System.Collections.Generic;

namespace PresenceSearchPlayer.Entity.Structure.Request
{

    public class UniqueSearchRequest : PSPRequestBase
    {
        public string PreferredNick { get; protected set; }
        public uint NamespaceID { get; protected set; }
        public UniqueSearchRequest(string rawRequest) :base(rawRequest)
        {
        }

        public string GameName { get; private set; }

        public override object Parse()
        {
            var flag = (GPErrorCode)base.Parse();
            if (flag != GPErrorCode.NoError)
            {
                return flag;
            }

            if (!RequestKeyValues.ContainsKey("preferrednick"))
            {
                return GPErrorCode.Parse;
            }

            PreferredNick = RequestKeyValues["preferrednick"];

            if (!RequestKeyValues.ContainsKey("gamename"))
            {
                return GPErrorCode.Parse;
            }
            GameName = RequestKeyValues["gamename"];

            if (RequestKeyValues.ContainsKey("namespaceid"))
            {
                uint namespaceID;
                if (!uint.TryParse(RequestKeyValues["namespaceid"], out namespaceID))
                {
                    return GPErrorCode.Parse;
                }

                NamespaceID = namespaceID;
            }

            return GPErrorCode.NoError;
        }
    }
}
