using UniSpyServer.PresenceSearchPlayer.Abstraction.BaseClass;
using UniSpyServer.PresenceSearchPlayer.Entity.Contract;
using UniSpyServer.PresenceSearchPlayer.Entity.Exception.General;
using System.Collections.Generic;
using System.Linq;

namespace UniSpyServer.PresenceSearchPlayer.Entity.Structure.Request
{

    //request: \otherslist\sesskey\<searcher's sesskey>\profileid\<searcher's pid>\numopids\<how many pid in his list>
    //\opids\|<opid1>|<opid2>|******\namespaceid\<>\gamename\<>\final\
    [RequestContract("otherslist")]
    public sealed class OthersListRequest : RequestBase
    {
        public List<uint> ProfileIDs { get; private set; }
        public uint NamespaceID { get; private set; }
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
