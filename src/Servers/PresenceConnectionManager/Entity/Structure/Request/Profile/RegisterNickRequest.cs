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

        public override void Parse()
        {
            base.Parse();
            if( ErrorCode != GPErrorCode.NoError)
            {
                return;
            }

            if (!KeyValues.ContainsKey("sesskey"))
            {
                ErrorCode = GPErrorCode.Parse; return;
            }

            if (!KeyValues.ContainsKey("uniquenick"))
            {
                ErrorCode = GPErrorCode.Parse; return;
            }

            UniqueNick = KeyValues["uniquenick"];

            ErrorCode = GPErrorCode.NoError;
        }
    }
}
