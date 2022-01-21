using UniSpyServer.Servers.PresenceSearchPlayer.Abstraction.BaseClass;

namespace UniSpyServer.Servers.PresenceSearchPlayer.Entity.Structure.Result
{
    public sealed class CheckResult : ResultBase
    {
        public int ProfileId { get; set; }
        public CheckResult()
        {
        }
    }
}
