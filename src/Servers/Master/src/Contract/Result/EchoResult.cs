using UniSpy.Server.Master.Abstraction.BaseClass;

namespace UniSpy.Server.Master.Contract.Result
{
    public sealed class EchoResult : ResultBase
    {
        public string Challenge { get; set; }
        public EchoResult()
        {
        }
    }
}