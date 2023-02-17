using UniSpy.Server.PresenceSearchPlayer.Enumerate;

namespace UniSpy.Server.PresenceSearchPlayer.Exception.Login
{
    public class GPLoginBadPasswordException : GPLoginException
    {
        public GPLoginBadPasswordException() : base("Password provided is invalid!", GPErrorCode.LoginBadPassword)
        {
        }

        public GPLoginBadPasswordException(string message) : base(message, GPErrorCode.LoginBadPassword)
        {
        }

        public GPLoginBadPasswordException(string message, System.Exception innerException) : base(message, GPErrorCode.LoginBadPassword, innerException)
        {
        }
    }
}