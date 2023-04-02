using UniSpy.Server.QueryReport.V1.Abstraction.BaseClass;

namespace UniSpy.Server.QueryReport.V1.Contract.Result
{
    public sealed class EchoResult : ResultBase
    {
        public string Challenge { get; set; }
        public EchoResult()
        {
        }
    }
}