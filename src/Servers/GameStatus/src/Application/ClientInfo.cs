using UniSpy.Server.Core.Abstraction.BaseClass;

namespace UniSpy.Server.GameStatus.Application
{
    public sealed class ClientInfo : ClientInfoBase
    {
        public const string ChallengeResponse = @"\challenge\00000000000000000000\final\";
        public int? SessionKey;
        public string GameName;
        public ClientInfo( )
        {
        }
    }
}