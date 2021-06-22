using GameStatus.Abstraction.BaseClass;

namespace GameStatus.Entity.Structure.Result
{
    internal sealed class AuthResult : GSResultBase
    {
        public uint SessionKey { get; set; }
        public AuthResult()
        {
        }
    }
}
