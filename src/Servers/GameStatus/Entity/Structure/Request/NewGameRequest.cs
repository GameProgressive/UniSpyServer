using GameStatus.Abstraction.BaseClass;
using GameStatus.Entity.Enumerate;
using System.Collections.Generic;

namespace GameStatus.Entity.Structure.Request
{
    public class NewGameRequest : GSRequestBase
    {
        public NewGameRequest(string rawRequest) : base(rawRequest)
        {
        }

        public override void Parse()
        {
           base.Parse();
            if (ErrorCode != GSErrorCode.NoError)
            {
                return;
            }

            ErrorCode = GSErrorCode.NoError;
        }
    }
}
