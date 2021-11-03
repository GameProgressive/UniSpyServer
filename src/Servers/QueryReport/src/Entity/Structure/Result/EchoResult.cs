using UniSpyServer.Servers.QueryReport.Abstraction.BaseClass;
using UniSpyServer.Servers.QueryReport.Entity.Structure.Redis;

namespace UniSpyServer.Servers.QueryReport.Entity.Structure.Result
{
    public sealed class EchoResult : ResultBase
    {
        public GameServerInfo Info { get; set; }
        public EchoResult()
        {
        }
    }
}
