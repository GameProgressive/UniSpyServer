using GameStatus.Abstraction.BaseClass;

namespace GameStatus.Entity.Structure.Result
{
    internal sealed class AuthResult : ResultBase
    {
        public uint SessionKey { get; set; }
        public AuthResult()
        {
        }
    }
}
