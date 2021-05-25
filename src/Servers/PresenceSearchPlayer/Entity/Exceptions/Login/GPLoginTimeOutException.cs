using PresenceSearchPlayer.Abstraction.BaseClass;
using PresenceSearchPlayer.Entity.Enumerate;

namespace PresenceSearchPlayer.Entity.Exceptions.Login
{
    public class GPLoginTimeOutException : GPExceptionBase
    {
        public GPLoginTimeOutException() : base("Login timeout!")
        {
            ErrorCode = GPErrorCode.LoginTimeOut;
        }

        public GPLoginTimeOutException(string message) : base(message)
        {
            ErrorCode = GPErrorCode.LoginTimeOut;
        }

        public GPLoginTimeOutException(string message, System.Exception innerException) : base(message, innerException)
        {
            ErrorCode = GPErrorCode.LoginTimeOut;
        }
    }
}
