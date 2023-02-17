using UniSpy.Server.GameStatus.Abstraction.BaseClass;

namespace UniSpy.Server.GameStatus.Entity.Structure.Result
{
    public sealed class GetProfileIDResult : ResultBase
    {
        public int ProfileId { get; set; }
        public GetProfileIDResult()
        {
        }
    }
}
