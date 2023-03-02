using UniSpy.Server.Core.Abstraction.BaseClass;

namespace UniSpy.Server.Master.Application
{
    public sealed class ClientInfo : ClientInfoBase
    {
        public string GameSecretKey { get; set; }
        public ClientInfo()
        {
        }
    }
}