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
        public SearchRequest(Dictionary<string, string> recv) : base(recv)
        {
        }

        public override GPErrorCode Parse()
        {
            var flag = base.Parse();
            if (flag != GPErrorCode.NoError)
            {
                return flag;
            }

            if (!_rawRequest.ContainsKey("profileid") && !_rawRequest.ContainsKey("namespaceid") && !_rawRequest.ContainsKey("gamename"))
            {
                return GPErrorCode.Parse;
            }

            GameName = _rawRequest["gamename"];

            uint profileID;
            if (!uint.TryParse(_rawRequest["profileid"], out profileID))
            {
                return GPErrorCode.Parse;
            }
            ProfileID = profileID;

            if (_rawRequest.ContainsKey("partnerid"))
            {
                uint partnerID;
                if (!uint.TryParse(_rawRequest["partnerid"], out partnerID))
                {
                    return GPErrorCode.Parse;
                }
                PartnerID = partnerID;
            }

            if (_rawRequest.ContainsKey("skip"))
            {
                int skip;
                if (!int.TryParse(_rawRequest["skip"], out skip))
                {
                    return GPErrorCode.Parse;
                }
                SkipNumber = skip;
            }

            if (_rawRequest.ContainsKey("uniquenick") && _rawRequest.ContainsKey("namespaceid"))
            {
                if (_rawRequest.ContainsKey("namespaceid"))
                {
                    uint namespaceID;
                    if (!uint.TryParse(_rawRequest["namespaceid"], out namespaceID))
                    {
                        return GPErrorCode.Parse;
                    }
                    NamespaceID = namespaceID;
                }
                RequestType = SearchRequestType.UniquenickNamespaceIDSearch;

                Uniquenick = _rawRequest["uniquenick"];
            }
            else if (_rawRequest.ContainsKey("nick") && _rawRequest.ContainsKey("email"))
            {
                RequestType = SearchRequestType.NickEmailSearch;
                Nick = _rawRequest["nick"];
                Email = _rawRequest["email"];
            }
            else if (_rawRequest.ContainsKey("nick"))
            {
                RequestType = SearchRequestType.NickSearch;
                Nick = _rawRequest["nick"];
            }
            else if (_rawRequest.ContainsKey("email"))
            {
                //\search\\sesskey\0\profileid\0\namespaceid\1\email\spyguy@gamespy.cn\gamename\conflictsopc\final\
                Email = _rawRequest["email"];
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
