using UniSpyServer.PresenceSearchPlayer.Entity.Enumerate;

namespace UniSpyServer.PresenceSearchPlayer.Entity.Exception.Login
{
    public class GPLoginServerAuthFailedException : GPLoginException
    {
        public GPLoginServerAuthFailedException() : base("Login server authentication failed!", GPErrorCode.LoginServerAuthFaild)
        {
        }

        public GPLoginServerAuthFailedException(string message) : base(message)
        {
        }

        public GPLoginServerAuthFailedException(string message, System.Exception innerException) : base(message, innerException)
        {
        }
    }
}