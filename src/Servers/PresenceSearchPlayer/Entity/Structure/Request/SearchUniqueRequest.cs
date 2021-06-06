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

            if (!RequestKeyValues.ContainsKey("uniquenick") || !RequestKeyValues.ContainsKey("namespaces"))
            {
                throw new GPGeneralException("searchunique request is incomplete.", GPErrorCode.Parse);
            }

            try
            {
                Uniquenick = RequestKeyValues["uniquenick"];
            }
            catch
            {
                throw new GPGeneralException("uniquenick is missing.", GPErrorCode.Parse);
            }

            try
            {
                Namespaces = RequestKeyValues["namespaces"].TrimStart(',').Split(',').Select(uint.Parse).ToList();
            }
            catch
            {
                throw new GPGeneralException("namespaces is incorrect.", GPErrorCode.Parse);
            }
        }
    }
}
