using PresenceSearchPlayer.Abstraction.BaseClass;
using PresenceSearchPlayer.Entity.Enumerate;

namespace PresenceSearchPlayer.Entity.Exceptions.Login
{
    public class GPLoginException : GPExceptionBase
    {
        public GPLoginException() : base("Unknown login error!")
        {
            ErrorCode = GPErrorCode.Login;
        }

        public GPLoginException(string message) : base(message)
        {
            ErrorCode = GPErrorCode.Login;
        }

        public GPLoginException(string message, System.Exception innerException) : base(message, innerException)
        {
            ErrorCode = GPErrorCode.Login;
        }
    }
}