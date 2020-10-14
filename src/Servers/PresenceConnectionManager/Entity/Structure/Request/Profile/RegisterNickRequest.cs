using PresenceConnectionManager.Entity.BaseClass;
using PresenceSearchPlayer.Entity.Enumerator;
using System.Collections.Generic;

namespace PresenceConnectionManager.Entity.Structure.Request.Profile
{
    public class RegisterNickRequest : PCMRequest
    {
        public string UniqueNick { get; protected set; }
        public RegisterNickRequest(Dictionary<string, string> recv) : base(recv)
        {
        }

        public override GPError Parse()
        {
            var flag = base.Parse();
            if (flag != GPError.NoError)
            {
                return flag;
            }

            if (!_recv.ContainsKey("sesskey"))
            {
                return GPError.Parse;
            }

            if (!_recv.ContainsKey("uniquenick"))
            {
                return GPError.Parse;
            }

            UniqueNick = _recv["uniquenick"];

            return GPError.NoError;
        }
    }
}
