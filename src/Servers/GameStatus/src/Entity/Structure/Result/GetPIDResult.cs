using UniSpyServer.GameStatus.Abstraction.BaseClass;

namespace UniSpyServer.GameStatus.Entity.Structure.Result
{
    public sealed class GetPIDResult : ResultBase
    {
        public uint ProfileID { get; set; }
        public GetPIDResult()
        {
        }
    }
}
