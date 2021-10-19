using GameStatus.Abstraction.BaseClass;

namespace GameStatus.Entity.Structure.Result
{
    public sealed class GetPIDResult : ResultBase
    {
        public uint ProfileID { get; set; }
        public GetPIDResult()
        {
        }
    }
}
