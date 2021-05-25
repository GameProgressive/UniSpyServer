using PresenceSearchPlayer.Abstraction.BaseClass;
using PresenceSearchPlayer.Entity.Enumerate;
namespace PresenceSearchPlayer.Entity.Exceptions.General
{
    public class GPNotLoggedInException : GPExceptionBase
    {
        public GPNotLoggedInException() : base("You are not logged in, please login first!")
        {
            ErrorCode = GPErrorCode.NotLoggedIn;
        }

        public GPNotLoggedInException(string message) : base(message)
        {
            ErrorCode = GPErrorCode.NotLoggedIn;
        }

        public GPNotLoggedInException(string message, System.Exception innerException) : base(message, innerException)
        {
            ErrorCode = GPErrorCode.NotLoggedIn;
        }
    }
}