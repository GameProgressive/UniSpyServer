using UniSpy.Server.PresenceSearchPlayer.Enumerate;

namespace UniSpy.Server.PresenceSearchPlayer.Exception.NewUser
{
    public class GPNewUserBadPasswordException : GPNewUserException
    {
        public GPNewUserBadPasswordException() : base("Password is invalid!", GPErrorCode.NewUserBadPasswords)
        {
        }

        public GPNewUserBadPasswordException(string message) : base(message, GPErrorCode.NewUserBadPasswords)
        {
        }

        public GPNewUserBadPasswordException(string message, System.Exception innerException) : base(message, GPErrorCode.NewUserBadPasswords, innerException)
        {
        }
    }
}