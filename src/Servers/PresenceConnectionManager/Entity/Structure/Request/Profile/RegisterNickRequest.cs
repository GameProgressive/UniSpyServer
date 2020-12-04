using PresenceConnectionManager.Abstraction.BaseClass;
using PresenceSearchPlayer.Entity.Enumerate;
using System.Collections.Generic;

namespace PresenceConnectionManager.Entity.Structure.Request.Profile
{
    public class RegisterNickRequest : PCMRequestBase
    {
        public string UniqueNick { get; protected set; }
        public RegisterNickRequest(string rawRequest) : base(rawRequest)
        {
        }

        public override object Parse()
        {
            var flag = (GPErrorCode)base.Parse();
            if (flag != GPErrorCode.NoError)
            {
                return flag;
            }

            if (!KeyValues.ContainsKey("sesskey"))
            {
                return GPErrorCode.Parse;
            }

            if (!KeyValues.ContainsKey("uniquenick"))
            {
                return GPErrorCode.Parse;
            }

            UniqueNick = KeyValues["uniquenick"];

            return GPErrorCode.NoError;
        }
    }
}
