using PresenceSearchPlayer.Entity.Enumerate;

namespace PresenceSearchPlayer.Entity.Exception.Login
{
    public class GPLoginConnectionFailedException : GPLoginException
    {
        public GPLoginConnectionFailedException() : base("Login connection failed.", GPErrorCode.LoginConnectionFailed)
        {
        }

        public GPLoginConnectionFailedException(string message) : base(message, GPErrorCode.LoginConnectionFailed)
        {
        }

        public GPLoginConnectionFailedException(string message, System.Exception innerException) : base(message, GPErrorCode.LoginConnectionFailed, innerException)
        {
        }
    }
}