using UniSpy.Server.PresenceConnectionManager.Abstraction.BaseClass;
using UniSpy.Server.PresenceSearchPlayer.Exception.General;

namespace UniSpy.Server.PresenceConnectionManager.Contract.Request
{

    public sealed class NewProfileRequest : RequestBase
    {
        public NewProfileRequest(string rawRequest) : base(rawRequest)
        {
        }

        public bool IsReplaceNickName { get; private set; }
        public string SessionKey { get; private set; }
        public string NewNick { get; private set; }
        public string OldNick { get; private set; }
        public override void Parse()
        {
            base.Parse();

            if (!RequestKeyValues.ContainsKey("sesskey"))
            {
                throw new GPParseException("sesskey is missing");
            }
            SessionKey = RequestKeyValues["sesskey"];

            if (RequestKeyValues.ContainsKey("replace"))
            {
                if (!RequestKeyValues.ContainsKey("oldnick") && !RequestKeyValues.ContainsKey("nick"))
                {
                    throw new GPParseException("oldnick or nick is missing.");
                }
                OldNick = RequestKeyValues["oldnick"];
                NewNick = RequestKeyValues["nick"];
                IsReplaceNickName = true;
            }
            else
            {
                if (!RequestKeyValues.ContainsKey("nick"))
                {
                    throw new GPParseException("nick is missing.");
                }
                NewNick = RequestKeyValues["nick"];
                IsReplaceNickName = false;
            }
        }
    }
}
