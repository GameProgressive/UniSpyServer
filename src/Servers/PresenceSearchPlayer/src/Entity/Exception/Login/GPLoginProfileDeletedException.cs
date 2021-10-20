using UniSpyServer.PresenceSearchPlayer.Entity.Enumerate;

namespace UniSpyServer.PresenceSearchPlayer.Entity.Exception.Login
{
    public class GPLoginProfileDeletedException : GPLoginException
    {
        public GPLoginProfileDeletedException() : base("User's profile has been deleted!", GPErrorCode.LoginProfileDeleted)
        {
        }

        public GPLoginProfileDeletedException(string message) : base(message, GPErrorCode.LoginProfileDeleted)
        {
        }

        public GPLoginProfileDeletedException(string message, System.Exception innerException) : base(message, GPErrorCode.LoginProfileDeleted, innerException)
        {
        }
    }
}