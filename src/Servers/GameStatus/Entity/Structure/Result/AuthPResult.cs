using GameStatus.Abstraction.BaseClass;

namespace GameStatus.Entity.Structure.Result
{
    internal sealed class AuthPResult : GSResultBase
    {
        public uint ProfileID { get; set; }
        public AuthPResult()
        {
        }
    }
}
