using UniSpy.Server.GameStatus.Abstraction.BaseClass;

namespace UniSpy.Server.GameStatus.Contract.Result
{
    public sealed class AuthGameResult : ResultBase
    {
        public int SessionKey { get; set; }
        public AuthGameResult()
        {
        }
    }
}
