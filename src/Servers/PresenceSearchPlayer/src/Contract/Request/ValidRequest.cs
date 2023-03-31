using UniSpy.Server.PresenceSearchPlayer.Abstraction.BaseClass;
using UniSpy.Server.PresenceSearchPlayer.Exception.General;
using UniSpy.Server.Core.Misc;

namespace UniSpy.Server.PresenceSearchPlayer.Contract.Request
{
    
    public sealed class ValidRequest : RequestBase
    {
        public ValidRequest(string rawRequest) : base(rawRequest)
        {
        }

        public int NamespaceID { get; private set; }
        public string Email { get; private set; }
        public string GameName { get; private set; }

        public override void Parse()
        {
            base.Parse();


            if (!RequestKeyValues.ContainsKey("email") && !GameSpyUtils.IsEmailFormatCorrect(RequestKeyValues["email"]))
            {
                throw new GPParseException("valid request is incomplete.");
            }

            Email = RequestKeyValues["email"];

            if (RequestKeyValues.ContainsKey("namespaceid"))
            {
                int namespaceID;
                if (!int.TryParse(RequestKeyValues["namespaceid"], out namespaceID))
                {
                    throw new GPParseException("namespaceid is incorrect.");
                }

                NamespaceID = namespaceID;
            }
        }
    }
}
