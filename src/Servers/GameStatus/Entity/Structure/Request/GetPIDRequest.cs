using GameStatus.Abstraction.BaseClass;
using GameStatus.Entity.Enumerate;
using System.Collections.Generic;

namespace GameStatus.Entity.Structure.Request
{
    public class GetPIDRequest : GSRequestBase
    {
        public string Nick { get; protected set; }
        public string KeyHash { get; protected set; }

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
