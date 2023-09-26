using UniSpy.Server.Core.Abstraction.BaseClass;

namespace UniSpy.Server.ServerBrowser.V1.Application
{
    public sealed class ClientInfo : ClientInfoBase
    {
        public const string Challenge = @"000000";
        public const string ChallengeResponse = $@"\basic\\secure\{Challenge}\final\";
        public string GameSecretKey { get; set; }
        public string ValidateKey { get; set; }
        public ClientInfo()
        {
        }
    }
}