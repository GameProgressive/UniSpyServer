using UniSpyServer.GameStatus.Abstraction.BaseClass;

namespace UniSpyServer.GameStatus.Entity.Structure.Result
{
    public sealed class AuthResult : ResultBase
    {
        public uint SessionKey { get; set; }
        public AuthResult()
        {
        }
    }
}
