using QueryReport.Abstraction.BaseClass;
using QueryReport.Entity.Structure.Redis;

namespace QueryReport.Entity.Structure.Result
{
    internal sealed class EchoResult : QRResultBase
    {
        public GameServerInfo GameServerInfo { get; set; }
        public EchoResult()
        {
        }
    }
}
