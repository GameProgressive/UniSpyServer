using PresenceSearchPlayer.Abstraction.BaseClass;

namespace PresenceSearchPlayer.Entity.Structure.Result
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
