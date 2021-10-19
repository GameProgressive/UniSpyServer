using GameStatus.Abstraction.BaseClass;

namespace GameStatus.Entity.Structure.Result
{
    public sealed class AuthPResult : ResultBase
    {
        public uint ProfileID { get; set; }
        public AuthPResult()
        {
        }
    }
}
