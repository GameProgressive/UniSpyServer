using UniSpy.Server.PresenceSearchPlayer.Abstraction.BaseClass;
using UniSpy.Server.PresenceSearchPlayer.Entity.Exception.General;

namespace UniSpy.Server.PresenceSearchPlayer.Entity.Structure.Request
{
    public enum SearchRequestType
    {
        NickSearch,
        NickEmailSearch,
        UniquenickNamespaceIDSearch,
        EmailSearch
    }

    
    public sealed class SearchRequest : RequestBase
    {
        public int SkipNum { get; private set; }
        public SearchRequestType RequestType { get; private set; }
        public string GameName { get; private set; }
        public int ProfileId { get; private set; }
        public int PartnerID { get; private set; }
        public string Email { get; private set; }
        public string Nick { get; private set; }
        public string Uniquenick { get; private set; }
        public int NamespaceID { get; private set; }
        public SearchRequest(string rawRequest) : base(rawRequest)
        {
        }

        public override void Parse()
        {
            base.Parse();


            if (!RequestKeyValues.ContainsKey("profileid")
                && !RequestKeyValues.ContainsKey("nick")
                && !RequestKeyValues.ContainsKey("email")
                && !RequestKeyValues.ContainsKey("namespaceid")
                && !RequestKeyValues.ContainsKey("gamename"))
            {
                throw new GPParseException(" Search request is incomplete.");
            }

            if (RequestKeyValues.ContainsKey("gamename"))
            {
                GameName = RequestKeyValues["gamename"];
            }

            if (RequestKeyValues.ContainsKey("profileid"))
            {
                int profileID;
                if (!int.TryParse(RequestKeyValues["profileid"], out profileID))
                {
                    throw new GPParseException("profileid is incorrect.");

                }
                ProfileId = profileID;
            }

            if (RequestKeyValues.ContainsKey("partnerid"))
            {
                int partnerID;
                if (!int.TryParse(RequestKeyValues["partnerid"], out partnerID))
                {
                    throw new GPParseException("partnerid is incorrect.");
                }
                PartnerID = partnerID;
            }

            if (RequestKeyValues.ContainsKey("skip"))
            {
                int skip;
                if (!int.TryParse(RequestKeyValues["skip"], out skip))
                {
                    throw new GPParseException("skip number is incorrect.");
                }
                SkipNum = skip;
            }

            if (RequestKeyValues.ContainsKey("uniquenick") && RequestKeyValues.ContainsKey("namespaceid"))
            {
                if (RequestKeyValues.ContainsKey("namespaceid"))
                {
                    int namespaceID;
                    if (!int.TryParse(RequestKeyValues["namespaceid"], out namespaceID))
                    {
                        throw new GPParseException("namespaceid is incorrect.");
                    }
                    NamespaceID = namespaceID;
                }
                RequestType = SearchRequestType.UniquenickNamespaceIDSearch;

                Uniquenick = RequestKeyValues["uniquenick"];
            }
            else if (RequestKeyValues.ContainsKey("nick") && RequestKeyValues.ContainsKey("email"))
            {
                RequestType = SearchRequestType.NickEmailSearch;
                Nick = RequestKeyValues["nick"];
                Email = RequestKeyValues["email"];
            }
            else if (RequestKeyValues.ContainsKey("nick"))
            {
                RequestType = SearchRequestType.NickSearch;
                Nick = RequestKeyValues["nick"];
            }
            else if (RequestKeyValues.ContainsKey("email"))
            {
                //\search\\sesskey\0\profileid\0\namespaceid\1\email\spyguy@gamespy.cn\gamename\conflictsopc\final\
                Email = RequestKeyValues["email"];
                RequestType = SearchRequestType.EmailSearch;
            }
            else
            {
                throw new GPParseException("unknown search request type.");
            }
        }
    }
}
