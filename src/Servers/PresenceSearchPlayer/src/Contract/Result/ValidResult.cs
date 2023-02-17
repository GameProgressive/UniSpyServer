using UniSpy.Server.PresenceSearchPlayer.Abstraction.BaseClass;

namespace UniSpy.Server.PresenceSearchPlayer.Contract.Result
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
