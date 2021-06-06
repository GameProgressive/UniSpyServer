﻿using PresenceSearchPlayer.Abstraction.BaseClass;
using PresenceSearchPlayer.Entity.Enumerate;
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
                throw new GPGeneralException("opids or namespaceid is missing.", GPErrorCode.Parse);
            }

            try
            {
                ProfileIDs = RequestKeyValues["opids"].TrimStart('|').Split('|').Select(uint.Parse).ToList();
            }
            catch (System.Exception e)
            {
                throw new GPGeneralException("opids is incorrect", GPErrorCode.Parse, e);
            }

            if (RequestKeyValues.ContainsKey("namespaceid"))
            {
                uint namespaceID;
                if (!uint.TryParse(RequestKeyValues["namespaceid"], out namespaceID))
                {
                    throw new GPGeneralException("namespaceid is incorrect.", GPErrorCode.Parse);
                }

                NamespaceID = namespaceID;
            }
        }
    }
}
