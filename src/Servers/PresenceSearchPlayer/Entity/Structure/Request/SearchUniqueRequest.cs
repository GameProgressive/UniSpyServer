using PresenceSearchPlayer.Abstraction.BaseClass;
using PresenceSearchPlayer.Entity.Enumerate;
using System.Collections.Generic;
using System.Linq;

namespace PresenceSearchPlayer.Entity.Structure.Request
{
    public class SearchUniqueRequest : PSPRequestBase
    {
        public string Uniquenick { get; private set; }
        public List<uint> Namespaces { get; protected set; }
        public SearchUniqueRequest(Dictionary<string, string> recv) : base(recv)
        {

        }

        public override GPErrorCode Parse()
        {
            var flag = base.Parse();
            if (flag != GPErrorCode.NoError)
            {
                return flag;
            }

            if (!_rawRequest.ContainsKey("uniquenick") || !_rawRequest.ContainsKey("namespaces"))
            {
                return GPErrorCode.Parse;
            }

            Uniquenick = _rawRequest["uniquenick"];
            Namespaces = _rawRequest["namespaces"].TrimStart(',').Split(',').Select(uint.Parse).ToList();

            return GPErrorCode.NoError;
        }
    }
}
