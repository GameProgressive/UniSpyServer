using GameStatus.Abstraction.BaseClass;

namespace GameStatus.Entity.Structure.Result
{
    internal sealed class AuthPResult : ResultBase
    {
        public uint ProfileID { get; set; }
        public AuthPResult()
        {
        }
    }
}
