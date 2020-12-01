using PresenceSearchPlayer.Abstraction.BaseClass;
using PresenceSearchPlayer.Entity.Enumerate;
using System.Collections.Generic;

namespace PresenceSearchPlayer.Entity.Structure.Request
{

    public class UniqueSearchRequest : PSPRequestBase
    {
        public string PreferredNick { get; protected set; }
        public uint NamespaceID { get; protected set; }
        public UniqueSearchRequest(Dictionary<string, string> recv) : base(recv)
        {
        }

        public string GameName { get; private set; }

        public override GPErrorCode Parse()
        {
            var flag = base.Parse();
            if (flag != GPErrorCode.NoError)
            {
                return flag;
            }

            if (!_rawRequest.ContainsKey("preferrednick"))
            {
                return GPErrorCode.Parse;
            }

            PreferredNick = _rawRequest["preferrednick"];

            if (!_rawRequest.ContainsKey("gamename"))
            {
                return GPErrorCode.Parse;
            }
            GameName = _rawRequest["gamename"];

            if (_rawRequest.ContainsKey("namespaceid"))
            {
                uint namespaceID;
                if (!uint.TryParse(_rawRequest["namespaceid"], out namespaceID))
                {
                    return GPErrorCode.Parse;
                }

                NamespaceID = namespaceID;
            }

            return GPErrorCode.NoError;
        }
    }
}
