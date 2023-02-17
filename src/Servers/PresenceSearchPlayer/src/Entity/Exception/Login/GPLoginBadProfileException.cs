using UniSpy.Server.PresenceSearchPlayer.Entity.Enumerate;

namespace UniSpy.Server.PresenceSearchPlayer.Entity.Exception.Login
{
    public class GPLoginBadProfileException : GPLoginException
    {
        public GPLoginBadProfileException() : base("User profile is damaged!", GPErrorCode.LoginBadProfile)
        {
        }

        public GPLoginBadProfileException(string message) : base(message, GPErrorCode.LoginBadProfile)
        {
        }

        public GPLoginBadProfileException(string message, System.Exception innerException) : base(message, GPErrorCode.LoginBadProfile, innerException)
        {
        }
    }
}