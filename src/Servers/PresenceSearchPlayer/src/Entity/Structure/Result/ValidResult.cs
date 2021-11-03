using UniSpyServer.Servers.PresenceSearchPlayer.Abstraction.BaseClass;

namespace UniSpyServer.Servers.PresenceSearchPlayer.Entity.Structure.Result
{
    public sealed class ValidResult : ResultBase
    {
        public bool IsAccountValid;
        public ValidResult()
        {
            IsAccountValid = false;
        }
    }
}
