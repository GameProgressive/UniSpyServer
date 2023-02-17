using UniSpy.Server.PresenceSearchPlayer.Enumerate;
using UniSpy.Server.PresenceSearchPlayer.Exception.General;

namespace UniSpy.Server.PresenceSearchPlayer.Exception.Login
{
    public class GPLoginBadPreAuthException : GPException
    {
        public GPLoginBadPreAuthException() : base("Login pre-authentication failed.", GPErrorCode.LoginBadPreAuth)
        {
        }

        public GPLoginBadPreAuthException(string message) : base(message, GPErrorCode.LoginBadPreAuth)
        {
        }

        public GPLoginBadPreAuthException(string message, System.Exception innerException) : base(message, GPErrorCode.LoginBadPreAuth, innerException)
        {
        }
    }
}