using PresenceConnectionManager.Abstraction.BaseClass;
using PresenceSearchPlayer.Entity.Enumerate;

namespace PresenceConnectionManager.Entity.Structure.Request.Profile
{
    internal sealed class RegisterNickRequest : PCMRequestBase
    {
        public string UniqueNick { get; private set; }
        public RegisterNickRequest(string rawRequest) : base(rawRequest)
        {
        }

        public override void Parse()
        {
            base.Parse();


            if (!KeyValues.ContainsKey("sesskey"))
            {
                ErrorCode = GPErrorCode.Parse; return;
            }

            if (!KeyValues.ContainsKey("uniquenick"))
            {
                ErrorCode = GPErrorCode.Parse; return;
            }

            UniqueNick = KeyValues["uniquenick"];
        }
    }
}
