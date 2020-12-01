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

        public override object Parse()
        {
            var flag = (GPErrorCode)base.Parse();
            if (flag != GPErrorCode.NoError)
            {
                return flag;
            }

            if (!_recv.ContainsKey("profileid") && !_recv.ContainsKey("namespaceid") && !_recv.ContainsKey("gamename"))
            {
                return GPErrorCode.Parse;
            }

            GameName = _recv["gamename"];

            uint profileID;
            if (!uint.TryParse(_recv["profileid"], out profileID))
            {
                return GPErrorCode.Parse;
            }
            ProfileID = profileID;

            if (_recv.ContainsKey("partnerid"))
            {
                uint partnerID;
                if (!uint.TryParse(_recv["partnerid"], out partnerID))
                {
                    return GPErrorCode.Parse;
                }
                PartnerID = partnerID;
            }

            if (_recv.ContainsKey("skip"))
            {
                int skip;
                if (!int.TryParse(_recv["skip"], out skip))
                {
                    return GPErrorCode.Parse;
                }
                SkipNumber = skip;
            }

            if (_recv.ContainsKey("uniquenick") && _recv.ContainsKey("namespaceid"))
            {
                if (_recv.ContainsKey("namespaceid"))
                {
                    uint namespaceID;
                    if (!uint.TryParse(_recv["namespaceid"], out namespaceID))
                    {
                        return GPErrorCode.Parse;
                    }
                    NamespaceID = namespaceID;
                }
                RequestType = SearchRequestType.UniquenickNamespaceIDSearch;

                Uniquenick = _recv["uniquenick"];
            }
            else if (_recv.ContainsKey("nick") && _recv.ContainsKey("email"))
            {
                RequestType = SearchRequestType.NickEmailSearch;
                Nick = _recv["nick"];
                Email = _recv["email"];
            }
            else if (_recv.ContainsKey("nick"))
            {
                RequestType = SearchRequestType.NickSearch;
                Nick = _recv["nick"];
            }
            else if (_recv.ContainsKey("email"))
            {
                //\search\\sesskey\0\profileid\0\namespaceid\1\email\spyguy@gamespy.cn\gamename\conflictsopc\final\
                Email = _recv["email"];
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
