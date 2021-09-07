using QueryReport.Abstraction.BaseClass;
using QueryReport.Entity.Structure.Redis;

namespace QueryReport.Entity.Structure.Result
{
    internal sealed class EchoResult : ResultBase
    {
        public GameServerInfo GameServerInfo { get; set; }
        public EchoResult()
        {
        }
    }
}
