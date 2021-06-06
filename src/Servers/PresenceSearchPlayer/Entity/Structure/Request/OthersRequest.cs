using PresenceSearchPlayer.Abstraction.BaseClass;
using PresenceSearchPlayer.Entity.Enumerate;

namespace PresenceSearchPlayer.Entity.Structure.Request
{
    internal class OthersRequest : PSPRequestBase
    {
        public OthersRequest(string rawRequest) : base(rawRequest)
        {
        }

        public uint ProfileID { get; private set; }
        public string GameName { get; private set; }
        public uint NamespaceID { get; protected set; }
        public override void Parse()
        {
            base.Parse();


            if (!RequestKeyValues.ContainsKey("gamename"))
            {
                throw new GPGeneralException("gamename is missing.", GPErrorCode.Parse);
            }

            if (!RequestKeyValues.ContainsKey("profileid") || !RequestKeyValues.ContainsKey("namespaceid"))
            {
                throw new GPGeneralException("profileid or namespaceid is missing.", GPErrorCode.Parse);
            }

            uint profileID = 0;
            if (!RequestKeyValues.ContainsKey("profileid") && !uint.TryParse(RequestKeyValues["profileid"], out profileID))
            {
                throw new GPGeneralException("profileid is incorrect.", GPErrorCode.Parse);
            }

            if (RequestKeyValues.ContainsKey("namespaceid"))
            {
                uint namespaceID;
                if (!uint.TryParse(RequestKeyValues["namespaceid"], out namespaceID))
                {
                    throw new GPGeneralException("namespaceid is incorrect.", GPErrorCode.Parse);
                }

                NamespaceID = namespaceID;
            }

            ProfileID = profileID;
            GameName = RequestKeyValues["gamename"];
        }
    }
}
