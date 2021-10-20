using UniSpyServer.GameStatus.Abstraction.BaseClass;

namespace UniSpyServer.GameStatus.Entity.Structure.Result
{
    public sealed class AuthPResult : ResultBase
    {
        public uint ProfileID { get; set; }
        public AuthPResult()
        {
        }
    }
}
