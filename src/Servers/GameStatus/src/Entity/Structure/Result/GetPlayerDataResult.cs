using UniSpy.Server.GameStatus.Abstraction.BaseClass;
using System.Collections.Generic;

namespace UniSpy.Server.GameStatus.Entity.Structure.Result
{
    public sealed class GetPlayerDataResult : ResultBase
    {
        public Dictionary<string, string> KeyValues { get; set; }
        public GetPlayerDataResult()
        {
        }
    }
}
