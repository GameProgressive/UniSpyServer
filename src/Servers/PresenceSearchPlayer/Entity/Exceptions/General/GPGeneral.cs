using PresenceSearchPlayer.Abstraction.BaseClass;
using PresenceSearchPlayer.Entity.Enumerate;

namespace PresenceSearchPlayer.Entity.Exceptions.General
{
    public class GPGeneralException : GPExceptionBase
    {
        public GPGeneralException() : base("General Error!")
        {
            ErrorCode = GPErrorCode.General;
        }

        public GPGeneralException(string message) : base(message)
        {
            ErrorCode = GPErrorCode.General;
        }

        public GPGeneralException(string message, System.Exception innerException) : base(message, innerException)
        {
            ErrorCode = GPErrorCode.General;
        }
    }
}