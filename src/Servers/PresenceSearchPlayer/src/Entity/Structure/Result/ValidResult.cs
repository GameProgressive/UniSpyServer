using UniSpyServer.PresenceSearchPlayer.Abstraction.BaseClass;

namespace UniSpyServer.PresenceSearchPlayer.Entity.Structure.Result
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
