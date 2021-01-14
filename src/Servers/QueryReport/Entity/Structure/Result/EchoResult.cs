using QueryReport.Abstraction.BaseClass;

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
