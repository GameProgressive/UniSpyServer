using UniSpyServer.GameStatus.Abstraction.BaseClass;
using UniSpyServer.GameStatus.Entity.Contract;
using UniSpyServer.GameStatus.Entity.Exception;

namespace UniSpyServer.GameStatus.Entity.Structure.Request
{
    [RequestContract("getpid")]
    public sealed class GetProfileIDRequest : RequestBase
    {
        public string Nick { get; private set; }
        public string KeyHash { get; private set; }

        public GetProfileIDRequest(string rawRequest) : base(rawRequest)
        {
        }

        public override void Parse()
        {
            base.Parse();


            if (!RequestKeyValues.ContainsKey("nick") || !RequestKeyValues.ContainsKey("keyhash"))
            {
                throw new GSException("nick or keyhash is missing.");
            }

            if (RequestKeyValues.ContainsKey("nick"))
            {
                Nick = RequestKeyValues["nick"];
            }
            else if (RequestKeyValues.ContainsKey("keyhash"))
            {
                KeyHash = RequestKeyValues["keyhash"];
            }
            else
            {
                throw new GSException("Unknown GetPID request type.");
            }
        }
    }
}
