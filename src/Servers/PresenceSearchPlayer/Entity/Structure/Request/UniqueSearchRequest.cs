using PresenceSearchPlayer.Abstraction.BaseClass;
using PresenceSearchPlayer.Entity.Contract;
using PresenceSearchPlayer.Entity.Exception.General;

namespace PresenceSearchPlayer.Entity.Structure.Request
{

    [RequestContract("uniquesearch")]
    internal sealed class UniqueSearchRequest : RequestBase
    {
        public string PreferredNick { get; private set; }
        public uint NamespaceID { get; private set; }
        public UniqueSearchRequest(string rawRequest) : base(rawRequest)
        {
        }

        public string GameName { get; private set; }

        public override void Parse()
        {
            base.Parse();


            if (!RequestKeyValues.ContainsKey("preferrednick"))
            {
                throw new GPParseException("preferrednick is missing.");
            }

            PreferredNick = RequestKeyValues["preferrednick"];

            if (!RequestKeyValues.ContainsKey("gamename"))
            {
                throw new GPParseException("gamename is missing.");
            }
            GameName = RequestKeyValues["gamename"];

            if (!RequestKeyValues.ContainsKey("namespaceid"))
            {
                throw new GPParseException("namespaceid is missing.");
            }

            uint namespaceID;
            if (!uint.TryParse(RequestKeyValues["namespaceid"], out namespaceID))
            {
                throw new GPParseException("namespaceid is incorrect.");
            }

            NamespaceID = namespaceID;
        }
    }
}
