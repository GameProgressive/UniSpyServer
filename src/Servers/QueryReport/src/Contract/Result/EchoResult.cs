using UniSpy.Server.QueryReport.V2.Abstraction.BaseClass;
using UniSpy.Server.QueryReport.V2.Aggregate.Redis.GameServer;

namespace UniSpy.Server.QueryReport.V2.Contract.Result
{
    public sealed class EchoResult : ResultBase
    {
        public GameServerInfo Info { get; set; }
        public EchoResult()
        {
        }
    }
}
