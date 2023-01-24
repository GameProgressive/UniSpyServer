using UniSpyServer.UniSpyLib.Abstraction.BaseClass;

namespace UniSpyServer.Servers.QueryReport.Application
{
    public sealed class ClientInfo : ClientInfoBase
    {
        public uint? InstantKey { get; set; }
        public ClientInfo( )
        {
        }
    }
}