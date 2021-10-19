using PresenceSearchPlayer.Abstraction.BaseClass;
using PresenceSearchPlayer.Entity.Contract;
using PresenceSearchPlayer.Entity.Exception.General;

namespace PresenceSearchPlayer.Entity.Structure.Request
{
    [RequestContract("others")]
    public sealed class OthersRequest : RequestBase
    {
        public OthersRequest(string rawRequest) : base(rawRequest)
        {
        }

        public uint ProfileID { get; private set; }
        public string GameName { get; private set; }
        public uint NamespaceID { get; private set; }
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

            uint profileID = 0;
            if (!RequestKeyValues.ContainsKey("profileid") && !uint.TryParse(RequestKeyValues["profileid"], out profileID))
            {
                throw new GPParseException("profileid is incorrect.");
            }

            if (RequestKeyValues.ContainsKey("namespaceid"))
            {
                uint namespaceID;
                if (!uint.TryParse(RequestKeyValues["namespaceid"], out namespaceID))
                {
                    throw new GPParseException("namespaceid is incorrect.");
                }

                NamespaceID = namespaceID;
            }

            ProfileID = profileID;
            GameName = RequestKeyValues["gamename"];
        }
    }
}
