using GameStatus.Abstraction.BaseClass;

namespace GameStatus.Entity.Structure.Result
{
    internal sealed class GetPIDResult : GSResultBase
    {
        public uint ProfileID { get; set; }
        public GetPIDResult()
        {
        }
    }
}
