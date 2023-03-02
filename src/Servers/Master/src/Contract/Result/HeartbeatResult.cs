using UniSpy.Server.Master.Abstraction.BaseClass;

namespace UniSpy.Server.Master.Contract.Result
{
    public sealed class HeartbeatResult : ResultBase
    {
        public string Challenge { get; set; }
        public HeartbeatResult()
        {
        }
    }
}