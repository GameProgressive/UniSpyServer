using UniSpyServer.Servers.PresenceSearchPlayer.Abstraction.BaseClass;

using UniSpyServer.Servers.PresenceSearchPlayer.Entity.Exception.General;
using System.Collections.Generic;
using System.Linq;

namespace UniSpyServer.Servers.PresenceSearchPlayer.Entity.Structure.Request
{

    //request: \otherslist\sesskey\<searcher's sesskey>\profileid\<searcher's pid>\numopids\<how many pid in his list>
    //\opids\|<opid1>|<opid2>|******\namespaceid\<>\gamename\<>\final\
    
    public sealed class OthersListRequest : RequestBase
    {
        public List<int> ProfileIDs { get; private set; }
        public int NamespaceID { get; private set; }
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
                ProfileIDs = RequestKeyValues["opids"].TrimStart('|').Split('|').Select(int.Parse).ToList();
            }
            catch (System.Exception e)
            {
                throw new GPParseException("opids is incorrect", e);
            }

            if (RequestKeyValues.ContainsKey("namespaceid"))
            {
                int namespaceID;
                if (!int.TryParse(RequestKeyValues["namespaceid"], out namespaceID))
                {
                    throw new GPParseException("namespaceid is incorrect.");
                }

                NamespaceID = namespaceID;
            }
        }
    }
}
