using System.Collections.Generic;
using System.Linq;
using PresenceSearchPlayer.Entity.Enumerator;
using PresenceSearchPlayer.Entity.Structure.Model;

namespace PresenceSearchPlayer.Entity.Structure.Request
{
    public class OthersListRequest : PSPRequestBase
    {
        public List<uint> ProfileIDs { get; protected set; }
        public uint NamespaceID { get; protected set; }
        public OthersListRequest(Dictionary<string, string> recv) : base(recv)
        {
        }

        public override GPError Parse()
        {
            var flag = base.Parse();
            if (flag != GPError.NoError)
            {
                return flag;
            }

            if (!_rawRequest.ContainsKey("opids") || !_rawRequest.ContainsKey("namespaceid"))
            {
                return GPError.Parse;
            }

            ProfileIDs = _rawRequest["opids"].TrimStart('|').Split('|').Select(uint.Parse).ToList();

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
