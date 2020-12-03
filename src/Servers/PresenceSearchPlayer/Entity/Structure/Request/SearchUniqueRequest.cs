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
        public SearchUniqueRequest(string rawRequest) :base(rawRequest)
        {

        }

        public override object Parse()
        {
            var flag = (GPErrorCode)base.Parse();
            if (flag != GPErrorCode.NoError)
            {
                return flag;
            }

            if (!RequestKeyValues.ContainsKey("uniquenick") || !RequestKeyValues.ContainsKey("namespaces"))
            {
                return GPErrorCode.Parse;
            }

            Uniquenick = RequestKeyValues["uniquenick"];
            Namespaces = RequestKeyValues["namespaces"].TrimStart(',').Split(',').Select(uint.Parse).ToList();

            return GPErrorCode.NoError;
        }
    }
}
