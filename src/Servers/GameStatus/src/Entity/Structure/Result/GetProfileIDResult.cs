using UniSpyServer.Servers.GameStatus.Abstraction.BaseClass;

namespace UniSpyServer.Servers.GameStatus.Entity.Structure.Result
{
    public sealed class GetProfileIDResult : ResultBase
    {
        public uint ProfileID { get; set; }
        public GetProfileIDResult()
        {
        }
    }
}
