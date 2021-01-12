using GameStatus.Abstraction.BaseClass;
using GameStatus.Entity.Enumerate;
using System.Collections.Generic;

namespace GameStatus.Entity.Structure.Request
{
    internal sealed class NewGameRequest : GSRequestBase
    {
        public NewGameRequest(string rawRequest) : base(rawRequest)
        {
        }
    }
}
