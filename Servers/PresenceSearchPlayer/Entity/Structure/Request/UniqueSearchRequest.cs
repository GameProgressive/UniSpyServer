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

        public override GPErrorCode Parse()
        {
            var flag = base.Parse();
            if (flag != GPErrorCode.NoError)
            {
                return flag;
            }

            if (!_recv.ContainsKey("preferrednick"))
            {
                return GPErrorCode.Parse;
            }

            PreferredNick = _recv["preferrednick"];

            if (!_recv.ContainsKey("gamename"))
            {
                return GPErrorCode.Parse;
            }
            GameName = _recv["gamename"];

            return GPErrorCode.NoError;
        }
    }
}
