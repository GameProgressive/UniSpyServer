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

        public override GPError Parse()
        {
            var flag = base.Parse();
            if (flag != GPError.NoError)
            {
                return flag;
            }

            if (!_rawRequest.ContainsKey("preferrednick"))
            {
                return GPError.Parse;
            }

            PreferredNick = _rawRequest["preferrednick"];

            if (!_rawRequest.ContainsKey("gamename"))
            {
                return GPError.Parse;
            }
            GameName = _rawRequest["gamename"];

            if (_rawRequest.ContainsKey("namespaceid"))
            {
                uint namespaceID;
                if (!uint.TryParse(_rawRequest["namespaceid"], out namespaceID))
                {
                    return GPError.Parse;
                }

                NamespaceID = namespaceID;
            }

            return GPError.NoError;
        }
    }
}
