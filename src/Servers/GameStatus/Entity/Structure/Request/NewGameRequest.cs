using GameStatus.Abstraction.BaseClass;

namespace GameStatus.Entity.Structure.Request
{
    internal sealed class NewGameRequest : GSRequestBase
    {
        public NewGameRequest(string rawRequest) : base(rawRequest)
        {
        }
    }
}
