using UniSpy.Server.PresenceSearchPlayer.Enumerate;

namespace UniSpy.Server.PresenceSearchPlayer.Exception.Login
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