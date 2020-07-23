using System;
using System.Collections.Generic;
using System.Linq;
using PresenceSearchPlayer.Entity.Structure.Model;
using PresenceSearchPlayer.Enumerator;

namespace PresenceSearchPlayer.Entity.Structure.Request
{
    public class SearchUniqueRequest : PSPRequestModelBase
    {
        public List<uint> Namespaces { get; protected set; }
        public SearchUniqueRequest(Dictionary<string, string> recv) : base(recv)
        {
           
        }

        public override GPErrorCode Parse()
        {
            var flag = base.Parse();
            if (flag != GPErrorCode.NoError)
            {
                return flag;
            }

            if (!_recv.ContainsKey("uniquenick") || !_recv.ContainsKey("namespaces"))
            {
                return GPErrorCode.Parse;
            }

            Uniquenick = _recv["uniquenick"];
            Namespaces = _recv["namespaces"].TrimStart(',').Split(',').Select(uint.Parse).ToList();

            return GPErrorCode.NoError;
        }
    }
}
