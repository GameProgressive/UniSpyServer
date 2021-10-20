using UniSpyServer.PresenceSearchPlayer.Abstraction.BaseClass;
using UniSpyServer.PresenceSearchPlayer.Entity.Contract;
using UniSpyServer.PresenceSearchPlayer.Entity.Exception.General;
using System.Collections.Generic;
using System.Linq;

namespace UniSpyServer.PresenceSearchPlayer.Entity.Structure.Request
{
    [RequestContract("searchunique")]
    public sealed class SearchUniqueRequest : RequestBase
    {
        public string Uniquenick { get; private set; }
        public List<uint> Namespaces { get; private set; }
        public SearchUniqueRequest(string rawRequest) : base(rawRequest)
        {
        }

        public override void Parse()
        {
            base.Parse();

            if (!RequestKeyValues.ContainsKey("uniquenick") || !RequestKeyValues.ContainsKey("namespaces"))
            {
                throw new GPParseException("searchunique request is incomplete.");
            }

            try
            {
                Uniquenick = RequestKeyValues["uniquenick"];
            }
            catch
            {
                throw new GPParseException("uniquenick is missing.");
            }

            try
            {
                Namespaces = RequestKeyValues["namespaces"].TrimStart(',').Split(',').Select(uint.Parse).ToList();
            }
            catch
            {
                throw new GPParseException("namespaces is incorrect.");
            }
        }
    }
}
