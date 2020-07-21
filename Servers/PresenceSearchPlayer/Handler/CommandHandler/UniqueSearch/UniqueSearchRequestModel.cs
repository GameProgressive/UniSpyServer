using System;
using System.Collections.Generic;
using PresenceSearchPlayer.Entity.Structure.Model;
using PresenceSearchPlayer.Enumerator;

namespace PresenceSearchPlayer.Handler.CommandHandler.UniqueSearch
{

    public class UniqueSearchRequestModel : PSPRequestModelBase
    {
        public string PreferredNick;
        public UniqueSearchRequestModel(Dictionary<string, string> recv) : base(recv)
        {
        }

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

            return GPErrorCode.NoError;
        }
    }
}
