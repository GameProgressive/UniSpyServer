using System;
using System.Collections.Generic;
using PresenceSearchPlayer.Entity.Enumerator;
using PresenceSearchPlayer.Entity.Structure.Model;
using PresenceSearchPlayer.Enumerator;

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
                return GPErrorCode.Parse;
            }

            return GPErrorCode.NoError;
        }
    }
}
