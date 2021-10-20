using UniSpyServer.PresenceSearchPlayer.Entity.Enumerate;
using UniSpyServer.PresenceSearchPlayer.Entity.Exception.General;

namespace UniSpyServer.PresenceSearchPlayer.Entity.Exception.Login
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