using UniSpy.Server.PresenceSearchPlayer.Abstraction.BaseClass;
using UniSpy.Server.PresenceSearchPlayer.Entity.Exception.General;
using System.Collections.Generic;
using System.Linq;

namespace UniSpy.Server.PresenceSearchPlayer.Entity.Structure.Request
{
    
    public sealed class SearchUniqueRequest : RequestBase
    {
        public string Uniquenick { get; private set; }
        public List<int> NamespaceIds { get; private set; }
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
                NamespaceIds = RequestKeyValues["namespaces"].TrimStart(',').Split(',').Select(int.Parse).ToList();
            }
            catch
            {
                throw new GPParseException("namespaces is incorrect.");
            }
        }
    }
}
