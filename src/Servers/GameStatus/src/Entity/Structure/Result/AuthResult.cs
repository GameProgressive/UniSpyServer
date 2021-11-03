using UniSpyServer.Servers.GameStatus.Abstraction.BaseClass;

namespace UniSpyServer.Servers.GameStatus.Entity.Structure.Result
{
    public sealed class AuthResult : ResultBase
    {
        public uint SessionKey { get; set; }
        public AuthResult()
        {
        }
    }
}
