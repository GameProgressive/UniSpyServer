using System.Collections.Generic;
using PresenceSearchPlayer.Entity.Enumerator;
using PresenceSearchPlayer.Entity.Structure.Model;

namespace PresenceSearchPlayer.Entity.Structure.Request
{
    public enum SearchRequestType
    {
        NickSearch,
        NickEmailSearch,
        UniquenickNamespaceIDSearch
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

            if (!_recv.ContainsKey("profileid") && !_recv.ContainsKey("namespaceid") && !_recv.ContainsKey("gamename"))
            {
                return GPError.Parse;
            }

            GameName = _recv["gamename"];

            uint profileID;
            if (!uint.TryParse(_recv["profileid"], out profileID))
            {
                return GPError.Parse;
            }
            ProfileID = profileID;

            if (_recv.ContainsKey("partnerid"))
            {
                uint partnerID;
                if (!uint.TryParse(_recv["partnerid"], out partnerID))
                {
                    return GPError.Parse;
                }
                PartnerID = partnerID;
            }

            if (_recv.ContainsKey("skip"))
            {
                int skip;
                if (!int.TryParse(_recv["skip"], out skip))
                {
                    return GPError.Parse;
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
                        return GPError.Parse;
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
                else
                {
                    return GPError.Parse;
                }

                return GPError.NoError;
            }
        }
    }
