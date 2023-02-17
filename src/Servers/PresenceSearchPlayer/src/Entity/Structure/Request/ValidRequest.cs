using UniSpy.Server.PresenceSearchPlayer.Abstraction.BaseClass;
using UniSpy.Server.PresenceSearchPlayer.Entity.Exception.General;
using UniSpy.Server.Core.MiscMethod;

namespace UniSpy.Server.PresenceSearchPlayer.Entity.Structure.Request
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
