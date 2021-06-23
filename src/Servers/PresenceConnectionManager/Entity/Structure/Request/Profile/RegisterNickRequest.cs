using PresenceConnectionManager.Abstraction.BaseClass;
using PresenceSearchPlayer.Entity.Exception.General;
using UniSpyLib.Abstraction.BaseClass;

namespace PresenceConnectionManager.Entity.Structure.Request.Profile
{
    [Command("registernick")]
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
                throw new GPParseException("sesskey is missing.");
            }

            if (!KeyValues.ContainsKey("uniquenick"))
            {
                throw new GPParseException("uniquenick is missing.");
            }
            SessionKey = KeyValues["sesskey"];
            UniqueNick = KeyValues["uniquenick"];
        }
    }
}
