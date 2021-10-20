using UniSpyServer.QueryReport.Abstraction.BaseClass;
using UniSpyServer.QueryReport.Entity.Structure.Redis;

namespace UniSpyServer.QueryReport.Entity.Structure.Result
{
    public sealed class EchoResult : ResultBase
    {
        public GameServerInfo Info { get; set; }
        public EchoResult()
        {
        }
    }
}
