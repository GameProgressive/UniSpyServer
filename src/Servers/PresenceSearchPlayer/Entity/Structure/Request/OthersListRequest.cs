using PresenceSearchPlayer.Abstraction.BaseClass;
using PresenceSearchPlayer.Entity.Enumerate;
using PresenceSearchPlayer.Entity.Exception.General;
using System.Collections.Generic;
using System.Linq;

namespace PresenceSearchPlayer.Entity.Structure.Request
{

    //request: \otherslist\sesskey\<searcher's sesskey>\profileid\<searcher's pid>\numopids\<how many pid in his list>
    //\opids\|<opid1>|<opid2>|******\namespaceid\<>\gamename\<>\final\
    internal class OthersListRequest : PSPRequestBase
    {
        public List<uint> ProfileIDs { get; protected set; }
        public uint NamespaceID { get; protected set; }
        public OthersListRequest(string rawRequest) : base(rawRequest)
        {
        }

        public override void Parse()
        {
            base.Parse();

            if (!RequestKeyValues.ContainsKey("opids") || !RequestKeyValues.ContainsKey("namespaceid"))
            {
                throw new GPParseException("opids or namespaceid is missing.");
            }

            try
            {
                ProfileIDs = RequestKeyValues["opids"].TrimStart('|').Split('|').Select(uint.Parse).ToList();
            }
            catch (System.Exception e)
            {
                throw new GPParseException("opids is incorrect", e);
            }

            if (RequestKeyValues.ContainsKey("namespaceid"))
            {
                uint namespaceID;
                if (!uint.TryParse(RequestKeyValues["namespaceid"], out namespaceID))
                {
                    throw new GPParseException("namespaceid is incorrect.");
                }

                NamespaceID = namespaceID;
            }
        }
    }
}
