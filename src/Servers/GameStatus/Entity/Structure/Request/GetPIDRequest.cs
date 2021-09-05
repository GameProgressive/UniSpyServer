using GameStatus.Abstraction.BaseClass;
using GameStatus.Entity.Exception;

namespace GameStatus.Entity.Structure.Request
{
    internal sealed class GetPIDRequest : GSRequestBase
    {
        public string Nick { get; private set; }
        public string KeyHash { get; private set; }

        public GetPIDRequest(string rawRequest) : base(rawRequest)
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
