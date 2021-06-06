using PresenceConnectionManager.Abstraction.BaseClass;
using PresenceSearchPlayer.Abstraction.BaseClass;
using PresenceSearchPlayer.Entity.Enumerate;

namespace PresenceConnectionManager.Entity.Structure.Request.Profile
{
    internal sealed class RegisterNickRequest : PCMRequestBase
    {
        public string UniqueNick { get; private set; }
        public string SessionKey { get; private set; }
        public RegisterNickRequest(string rawRequest) : base(rawRequest)
        {
        }

        public override void Parse()
        {
            base.Parse();


            if (!KeyValues.ContainsKey("sesskey"))
            {
                throw new GPGeneralException("sesskey is missing.", GPErrorCode.Parse);
            }

            if (!KeyValues.ContainsKey("uniquenick"))
            {
                throw new GPGeneralException("uniquenick is missing.", GPErrorCode.Parse);
            }
            SessionKey = KeyValues["sesskey"];
            UniqueNick = KeyValues["uniquenick"];
        }
    }
}
