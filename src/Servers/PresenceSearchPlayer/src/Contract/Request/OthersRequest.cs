using UniSpy.Server.PresenceSearchPlayer.Abstraction.BaseClass;
using UniSpy.Server.PresenceSearchPlayer.Exception.General;

namespace UniSpy.Server.PresenceSearchPlayer.Contract.Request
{
    
    public sealed class OthersRequest : RequestBase
    {
        public OthersRequest(string rawRequest) : base(rawRequest)
        {
        }

        public int ProfileId { get; private set; }
        public string GameName { get; private set; }
        public int NamespaceID { get; private set; }
        public override void Parse()
        {
            base.Parse();


            if (!RequestKeyValues.ContainsKey("gamename"))
            {
                throw new GPParseException("gamename is missing.");
            }

            if (!RequestKeyValues.ContainsKey("profileid") || !RequestKeyValues.ContainsKey("namespaceid"))
            {
                throw new GPParseException("profileid or namespaceid is missing.");
            }

            int profileID = 0;
            if (!RequestKeyValues.ContainsKey("profileid") && !int.TryParse(RequestKeyValues["profileid"], out profileID))
            {
                throw new GPParseException("profileid is incorrect.");
            }

            if (RequestKeyValues.ContainsKey("namespaceid"))
            {
                int namespaceID;
                if (!int.TryParse(RequestKeyValues["namespaceid"], out namespaceID))
                {
                    throw new GPParseException("namespaceid is incorrect.");
                }

                NamespaceID = namespaceID;
            }

            ProfileId = profileID;
            GameName = RequestKeyValues["gamename"];
        }
    }
}
