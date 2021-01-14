using GameStatus.Abstraction.BaseClass;
using GameStatus.Entity.Enumerate;

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
            if (ErrorCode != GSErrorCode.NoError)
            {
                return;
            }

            if (!RequestKeyValues.ContainsKey("nick") || !RequestKeyValues.ContainsKey("keyhash"))
            {
                ErrorCode = GSErrorCode.Parse;
                return;
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
                ErrorCode = GSErrorCode.Parse;
                return;
            }

            ErrorCode = GSErrorCode.NoError;
        }
    }
}
