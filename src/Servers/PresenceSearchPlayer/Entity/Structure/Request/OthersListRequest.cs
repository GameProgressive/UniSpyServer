using PresenceSearchPlayer.Abstraction.BaseClass;
using PresenceSearchPlayer.Entity.Enumerate;
using System.Collections.Generic;
using System.Linq;

namespace PresenceSearchPlayer.Entity.Structure.Request
{
    public class OthersListRequest : PSPRequestBase
    {
        public List<uint> ProfileIDs { get; protected set; }
        public uint NamespaceID { get; protected set; }
        public OthersListRequest(Dictionary<string, string> recv) : base(recv)
        {
        }

        public override GPErrorCode Parse()
        {
            var flag = base.Parse();
            if (flag != GPErrorCode.NoError)
            {
                return flag;
            }

            if (!_rawRequest.ContainsKey("opids") || !_rawRequest.ContainsKey("namespaceid"))
            {
                return GPErrorCode.Parse;
            }

            ProfileIDs = _rawRequest["opids"].TrimStart('|').Split('|').Select(uint.Parse).ToList();

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
