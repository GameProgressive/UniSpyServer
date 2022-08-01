using UniSpyServer.Servers.PresenceSearchPlayer.Entity.Enumerate;

namespace UniSpyServer.Servers.PresenceSearchPlayer.Entity.Exception.Login
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