using System.Collections.Generic;
using PresenceSearchPlayer.Entity.Enumerator;
using PresenceSearchPlayer.Entity.Structure.Model;

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

        public override GPError Parse()
        {
            var flag = base.Parse();
            if (flag != GPError.NoError)
            {
                return flag;
            }

            if (!_rawRequest.ContainsKey("profileid") && !_rawRequest.ContainsKey("namespaceid") && !_rawRequest.ContainsKey("gamename"))
            {
                return GPError.Parse;
            }

            GameName = _rawRequest["gamename"];

            uint profileID;
            if (!uint.TryParse(_rawRequest["profileid"], out profileID))
            {
                return GPError.Parse;
            }
            ProfileID = profileID;

            if (_rawRequest.ContainsKey("partnerid"))
            {
                uint partnerID;
                if (!uint.TryParse(_rawRequest["partnerid"], out partnerID))
                {
                    return GPError.Parse;
                }
                PartnerID = partnerID;
            }

            if (_rawRequest.ContainsKey("skip"))
            {
                int skip;
                if (!int.TryParse(_rawRequest["skip"], out skip))
                {
                    return GPError.Parse;
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
                        return GPError.Parse;
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
                return GPError.Parse;
            }

            return GPError.NoError;
        }
    }
}
