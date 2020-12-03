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
        public int SkipNumber { get; protected set; }
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

        public override object Parse()
        {
            var flag = (GPErrorCode)base.Parse();
            if (flag != GPErrorCode.NoError)
            {
                return flag;
            }

            if (!RequestKeyValues.ContainsKey("profileid") && !RequestKeyValues.ContainsKey("namespaceid") && !RequestKeyValues.ContainsKey("gamename"))
            {
                return GPErrorCode.Parse;
            }

            GameName = RequestKeyValues["gamename"];

            uint profileID;
            if (!uint.TryParse(RequestKeyValues["profileid"], out profileID))
            {
                return GPErrorCode.Parse;
            }
            ProfileID = profileID;

            if (RequestKeyValues.ContainsKey("partnerid"))
            {
                uint partnerID;
                if (!uint.TryParse(RequestKeyValues["partnerid"], out partnerID))
                {
                    return GPErrorCode.Parse;
                }
                PartnerID = partnerID;
            }

            if (RequestKeyValues.ContainsKey("skip"))
            {
                int skip;
                if (!int.TryParse(RequestKeyValues["skip"], out skip))
                {
                    return GPErrorCode.Parse;
                }
                SkipNumber = skip;
            }

            if (RequestKeyValues.ContainsKey("uniquenick") && RequestKeyValues.ContainsKey("namespaceid"))
            {
                if (RequestKeyValues.ContainsKey("namespaceid"))
                {
                    uint namespaceID;
                    if (!uint.TryParse(RequestKeyValues["namespaceid"], out namespaceID))
                    {
                        return GPErrorCode.Parse;
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
                return GPErrorCode.Parse;
            }

            return GPErrorCode.NoError;
        }
    }
}
