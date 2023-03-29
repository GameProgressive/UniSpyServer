using UniSpy.Server.Core.Abstraction.BaseClass;
using UniSpy.Server.Core.Encryption;

namespace UniSpy.Server.GameStatus.Application
{
    public sealed class ClientInfo : ClientInfoBase
    {
        public const string ChallengeResponse = @"\challenge\00000000000000000000\final\";
        public static byte[] ChallengeResponseBytes => UniSpyEncoding.GetBytes(ClientInfo.ChallengeResponse);
        public int? SessionKey { get; set; }
        public string GameName { get; set; }
        public bool IsUserAuthenticated { get; set; }
        public bool IsPlayerAuthenticated { get; set; }
        public bool IsGameAuthenticated { get; set; }
        public int? ProfileId { get; set; }
        public int? GameSessionKey { get; set; }
        public ClientInfo()
        {
        }
    }
}