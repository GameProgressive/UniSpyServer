using PresenceSearchPlayer.Abstraction.BaseClass;
using PresenceSearchPlayer.Entity.Enumerate;

namespace PresenceSearchPlayer.Entity.Exceptions.Login
{
    public class GPLoginBadNickException : GPExceptionBase
    {
        public GPLoginBadNickException() : base("Nickname is in valid!")
        {
            ErrorCode = GPErrorCode.LoginBadNick;
        }

        public GPLoginBadNickException(string message) : base(message)
        {
            ErrorCode = GPErrorCode.LoginBadNick;
        }

        public GPLoginBadNickException(string message, System.Exception innerException) : base(message, innerException)
        {
            ErrorCode = GPErrorCode.LoginBadNick;
        }
    }
}