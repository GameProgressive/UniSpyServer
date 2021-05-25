using PresenceSearchPlayer.Abstraction.BaseClass;
using PresenceSearchPlayer.Entity.Enumerate;

namespace PresenceSearchPlayer.Entity.Exceptions.Login
{
    public class GPLoginBadEmailException : GPExceptionBase
    {
        public GPLoginBadEmailException() : base("Email provided is invalid!")
        {
            ErrorCode = GPErrorCode.LoginBadEmail;
        }

        public GPLoginBadEmailException(string message) : base(message)
        {
            ErrorCode = GPErrorCode.LoginBadEmail;
        }

        public GPLoginBadEmailException(string message, System.Exception innerException) : base(message, innerException)
        {
            ErrorCode = GPErrorCode.LoginBadEmail;
        }
    }
}