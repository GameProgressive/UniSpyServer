using UniSpy.Server.Core.Abstraction.BaseClass;

namespace UniSpy.Server.ServerBrowser.V1.Application
{
    public sealed class ClientInfo : ClientInfoBase
    {
        public static string EncKey = @"000000";
        public static string ChallengeResponse = $@"\basic\\secure\{EncKey}\final\";
        public string GameSecretKey { get; set; }
        public ClientInfo()
        {
        }
    }
}