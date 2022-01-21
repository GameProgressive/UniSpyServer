using UniSpyServer.Servers.GameStatus.Abstraction.BaseClass;

namespace UniSpyServer.Servers.GameStatus.Entity.Structure.Result
{
    public sealed class AuthGameResult : ResultBase
    {
        public int SessionKey { get; set; }
        public AuthGameResult()
        {
        }
    }
}
