using UniSpyServer.Servers.PresenceSearchPlayer.Entity.Enumerate;

namespace UniSpyServer.Servers.PresenceSearchPlayer.Entity.Exception.Login
{
    public class GPLoginBadEmailException : GPLoginException
    {
        public GPLoginBadEmailException() : base("Email provided is invalid!", GPErrorCode.LoginBadEmail)
        {
        }

        public GPLoginBadEmailException(string message) : base(message, GPErrorCode.LoginBadEmail)
        {
        }

        public GPLoginBadEmailException(string message, System.Exception innerException) : base(message, GPErrorCode.LoginBadEmail, innerException)
        {
        }
    }
}