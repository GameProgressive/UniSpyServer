using UniSpy.Server.PresenceSearchPlayer.Abstraction.BaseClass;
using UniSpy.Server.PresenceSearchPlayer.Exception.General;
using UniSpy.Server.Core.MiscMethod;

namespace UniSpy.Server.PresenceSearchPlayer.Contract.Request
{
    
    public sealed class NicksRequest : RequestBase
    {
        public NicksRequest(string rawRequest) : base(rawRequest)
        {
        }

        public string Password { get; private set; }
        public string Email { get; private set; }
        public int NamespaceID { get; private set; }
        public bool IsRequireUniqueNicks { get; private set; }

        public override void Parse()
        {
            base.Parse();
            Password = PasswordEncoder.ProcessPassword(RequestKeyValues);

            if (!RequestKeyValues.ContainsKey("email"))
            {
                throw new GPParseException("email is missing.");
            }

            IsRequireUniqueNicks = true;

            if (RequestKeyValues.ContainsKey("pass"))
            {
                // Old games might send an error is unique nicknames are sent (like GSA 1.0)
                IsRequireUniqueNicks = false;
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
