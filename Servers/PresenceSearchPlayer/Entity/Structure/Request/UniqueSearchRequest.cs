using System.Collections.Generic;
using PresenceSearchPlayer.Entity.Enumerator;
using PresenceSearchPlayer.Entity.Structure.Model;

namespace PresenceSearchPlayer.Entity.Structure.Request
{

    public class UniqueSearchRequest : PSPRequestBase
    {
        public string PreferredNick;
        public UniqueSearchRequest(Dictionary<string, string> recv) : base(recv)
        {
        }

        public string GameName { get; private set; }

        public override GPError Parse()
        {
            var flag = base.Parse();
            if (flag != GPError.NoError)
            {
                return flag;
            }

            if (!_recv.ContainsKey("preferrednick"))
            {
                return GPError.Parse;
            }

            PreferredNick = _recv["preferrednick"];

            if (!_recv.ContainsKey("gamename"))
            {
                return GPError.Parse;
            }
            GameName = _recv["gamename"];

            return GPError.NoError;
        }
    }
}
