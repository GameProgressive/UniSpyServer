using UniSpy.Server.Core.Abstraction.BaseClass;

namespace UniSpy.Server.QueryReport.Application
{
    public sealed class ClientInfo : ClientInfoBase
    {
        public uint? InstantKey { get; set; }
        public ClientInfo( )
        {
        }
    }
}