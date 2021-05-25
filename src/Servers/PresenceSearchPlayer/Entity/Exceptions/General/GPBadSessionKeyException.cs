using PresenceSearchPlayer.Abstraction.BaseClass;
using PresenceSearchPlayer.Entity.Enumerate;

namespace PresenceSearchPlayer.Entity.Exceptions.General
{
    public class GPBadSessionKeyException : GPExceptionBase
    {
        public GPBadSessionKeyException() : base("Your session key is not valid!")
        {
            ErrorCode = GPErrorCode.BadSessionKey;
        }

        public GPBadSessionKeyException(string message) : base(message)
        {
            ErrorCode = GPErrorCode.BadSessionKey;
        }

        public GPBadSessionKeyException(string message, System.Exception innerException) : base(message, innerException)
        {
            ErrorCode = GPErrorCode.BadSessionKey;
        }
    }
}
