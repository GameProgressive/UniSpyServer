using PresenceSearchPlayer.Abstraction.BaseClass;
using PresenceSearchPlayer.Entity.Enumerate;
using System.Collections.Generic;

namespace PresenceSearchPlayer.Entity.Structure.Request
{
    public enum SearchRequestType
    {
        NickSearch,
        NickEmailSearch,
        UniquenickNamespaceIDSearch,
        EmailSearch
    }


    public class SearchRequest : PSPRequestBase
    {
        public int SkipNum { get; protected set; }
        public SearchRequestType RequestType { get; protected set; }
        public string GameName { get; private set; }
        public uint ProfileID { get; private set; }
        public uint PartnerID { get; private set; }
        public string Email { get; private set; }
        public string Nick { get; private set; }
        public string Uniquenick { get; private set; }
        public uint NamespaceID { get; protected set; }
        public SearchRequest(string rawRequest) :base(rawRequest)
        {
        }

        public override void Parse()
        {
            base.Parse();
            if (ErrorCode != GPErrorCode.NoError)
            {
                return;
            }

            if (!RequestKeyValues.ContainsKey("profileid")
                && !RequestKeyValues.ContainsKey("nick")
                && !RequestKeyValues.ContainsKey("email")
                && !RequestKeyValues.ContainsKey("namespaceid") 
                && !RequestKeyValues.ContainsKey("gamename"))
            {
                ErrorCode = GPErrorCode.Parse;
                return;
            }

            if (RequestKeyValues.ContainsKey("gamename"))
            {
                GameName = RequestKeyValues["gamename"];
            }

            if (RequestKeyValues.ContainsKey("profileid"))
            {
                uint profileID;
                if (!uint.TryParse(RequestKeyValues["profileid"], out profileID))
                {
                    ErrorCode = GPErrorCode.Parse;
                    return;
                }
                ProfileID = profileID;
            }

            if (RequestKeyValues.ContainsKey("partnerid"))
            {
                uint partnerID;
                if (!uint.TryParse(RequestKeyValues["partnerid"], out partnerID))
                {
                    ErrorCode = GPErrorCode.Parse;
                    return;
                }
                PartnerID = partnerID;
            }

            if (RequestKeyValues.ContainsKey("skip"))
            {
                int skip;
                if (!int.TryParse(RequestKeyValues["skip"], out skip))
                {
                    ErrorCode = GPErrorCode.Parse;
                    return;
                }
                SkipNum = skip;
            }

            if (RequestKeyValues.ContainsKey("uniquenick") && RequestKeyValues.ContainsKey("namespaceid"))
            {
                if (RequestKeyValues.ContainsKey("namespaceid"))
                {
                    uint namespaceID;
                    if (!uint.TryParse(RequestKeyValues["namespaceid"], out namespaceID))
                    {
                        ErrorCode = GPErrorCode.Parse;
                        return;
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
                ErrorCode = GPErrorCode.Parse;
                return;
            }

            ErrorCode = GPErrorCode.NoError;
        }
    }
}
