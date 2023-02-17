using UniSpy.Server.PresenceSearchPlayer.Abstraction.BaseClass;

namespace UniSpy.Server.PresenceSearchPlayer.Contract.Result
{
    public sealed class CheckResult : ResultBase
    {
        public int? ProfileId { get; set; }
        public CheckResult()
        {
        }
    }
}
