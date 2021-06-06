using PresenceSearchPlayer.Abstraction.BaseClass;
using PresenceSearchPlayer.Entity.Enumerate;

namespace PresenceSearchPlayer.Entity.Structure.Request
{

    internal class UniqueSearchRequest : PSPRequestBase
    {
        public string PreferredNick { get; protected set; }
        public uint NamespaceID { get; protected set; }
        public UniqueSearchRequest(string rawRequest) : base(rawRequest)
        {
        }

        public string GameName { get; private set; }

        public override void Parse()
        {
            base.Parse();


            if (!RequestKeyValues.ContainsKey("preferrednick"))
            {
                throw new GPGeneralException("preferrednick is missing.", GPErrorCode.Parse);
            }

            PreferredNick = RequestKeyValues["preferrednick"];

            if (!RequestKeyValues.ContainsKey("gamename"))
            {
                throw new GPGeneralException("gamename is missing.", GPErrorCode.Parse);
            }
            GameName = RequestKeyValues["gamename"];

            if (!RequestKeyValues.ContainsKey("namespaceid"))
            {
                throw new GPGeneralException("namespaceid is missing.", GPErrorCode.Parse);
            }

            uint namespaceID;
            if (!uint.TryParse(RequestKeyValues["namespaceid"], out namespaceID))
            {
                throw new GPGeneralException("namespaceid is incorrect.", GPErrorCode.Parse);
            }

            NamespaceID = namespaceID;
        }
    }
}
