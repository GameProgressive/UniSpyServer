using GameStatus.Abstraction.BaseClass;
using System.Collections.Generic;

namespace GameStatus.Entity.Structure.Result
{
    internal sealed class GetPDResult : GSResultBase
    {
        public Dictionary<string, string> KeyValues { get; set; }
        public GetPDResult()
        {
        }
    }
}
