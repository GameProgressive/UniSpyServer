using UniSpyServer.PresenceSearchPlayer.Abstraction.BaseClass;
using UniSpyServer.PresenceSearchPlayer.Entity.Contract;
using UniSpyServer.PresenceSearchPlayer.Entity.Exception.General;
using UniSpyServer.UniSpyLib.MiscMethod;

namespace UniSpyServer.PresenceSearchPlayer.Entity.Structure.Request
{
    [RequestContract("valid")]
    public sealed class ValidRequest : RequestBase
    {
        public ValidRequest(string rawRequest) : base(rawRequest)
        {
        }

        public uint NamespaceID { get; private set; }
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
                uint namespaceID;
                if (!uint.TryParse(RequestKeyValues["namespaceid"], out namespaceID))
                {
                    throw new GPParseException("namespaceid is incorrect.");
                }

                NamespaceID = namespaceID;
            }
        }
    }
}
