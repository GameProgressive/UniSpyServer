using UniSpyLib.MiscMethod;
using PresenceSearchPlayer.Abstraction.BaseClass;
using PresenceSearchPlayer.Entity.Enumerate;

namespace PresenceSearchPlayer.Entity.Structure.Request
{
    public class ValidRequest : PSPRequestBase
    {
        public ValidRequest(string rawRequest) :base(rawRequest)
        {
        }

        public uint NamespaceID { get; protected set; }
        public string Email { get; private set; }
        public string GameName { get; protected set; }

        public override void Parse()
        {
            base.Parse();
            if (ErrorCode != GPErrorCode.NoError)
            {
                return;
            }

            if (!RequestKeyValues.ContainsKey("email") && !GameSpyUtils.IsEmailFormatCorrect(RequestKeyValues["email"]))
            {
                ErrorCode = GPErrorCode.Parse;
                return;
            }

            Email = RequestKeyValues["email"];

            if (RequestKeyValues.ContainsKey("namespaceid"))
            {
                uint namespaceID;
                if (!uint.TryParse(RequestKeyValues["namespaceid"], out namespaceID))
                {
                    ErrorCode = GPErrorCode.Parse;
                    return;
                }

                NamespaceID = namespaceID;
            }
            ErrorCode = GPErrorCode.NoError;
        }
    }
}
