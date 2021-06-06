using PresenceSearchPlayer.Abstraction.BaseClass;
using PresenceSearchPlayer.Entity.Enumerate;
using System.Collections.Generic;
using System.Linq;

namespace PresenceSearchPlayer.Entity.Structure.Request
{
    internal class SearchUniqueRequest : PSPRequestBase
    {
        public string Uniquenick { get; private set; }
        public List<uint> Namespaces { get; protected set; }
        public SearchUniqueRequest(string rawRequest) : base(rawRequest)
        {

        }

        public override void Parse()
        {
            base.Parse();
            if (ErrorCode != GPErrorCode.NoError)
            {
                return;
            }

            if (!RequestKeyValues.ContainsKey("uniquenick") || !RequestKeyValues.ContainsKey("namespaces"))
            {
                ErrorCode = GPErrorCode.Parse;
                return;
            }

            Uniquenick = RequestKeyValues["uniquenick"];
            Namespaces = RequestKeyValues["namespaces"].TrimStart(',').Split(',').Select(uint.Parse).ToList();
        }
    }
}
