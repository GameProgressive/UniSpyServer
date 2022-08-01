using UniSpyServer.Servers.PresenceSearchPlayer.Entity.Enumerate;
using UniSpyServer.Servers.PresenceSearchPlayer.Entity.Exception.General;

namespace UniSpyServer.Servers.PresenceSearchPlayer.Entity.Exception.Login
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