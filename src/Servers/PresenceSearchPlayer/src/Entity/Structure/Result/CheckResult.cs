using UniSpyServer.PresenceSearchPlayer.Abstraction.BaseClass;

namespace UniSpyServer.PresenceSearchPlayer.Entity.Structure.Result
{
    public sealed class CheckResult : ResultBase
    {
        public uint ProfileID { get; set; }
        public CheckResult()
        {
        }
    }
}
