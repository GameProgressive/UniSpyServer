using System;
using System.Collections.Generic;
using PresenceConnectionManager.Entity.BaseClass;
using PresenceSearchPlayer.Entity.Enumerator;

namespace PresenceConnectionManager.Entity.Structure.Request.Profile
{
    public class RegisterNickRequest : PCMRequestBase
    {
        public string UniqueNick { get; protected set; }
        public RegisterNickRequest(Dictionary<string, string> recv) : base(recv)
        {
        }

        public override GPErrorCode Parse()
        {
            var flag = base.Parse();
            if (flag != GPErrorCode.NoError)
            {
                return flag;
            }

            if (!_recv.ContainsKey("sesskey"))
            {
                return GPErrorCode.Parse;
            }

            if (!_recv.ContainsKey("uniquenick"))
            {
                return GPErrorCode.Parse;
            }

            UniqueNick = _recv["uniquenick"];

            return GPErrorCode.NoError;
        }
    }
}
