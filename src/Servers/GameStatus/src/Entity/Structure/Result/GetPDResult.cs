using UniSpyServer.GameStatus.Abstraction.BaseClass;
using System.Collections.Generic;

namespace UniSpyServer.GameStatus.Entity.Structure.Result
{
    public sealed class GetPDResult : ResultBase
    {
        public Dictionary<string, string> KeyValues { get; set; }
        public GetPDResult()
        {
        }
    }
}
