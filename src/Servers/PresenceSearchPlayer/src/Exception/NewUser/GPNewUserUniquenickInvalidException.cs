using UniSpy.Server.PresenceSearchPlayer.Enumerate;

namespace UniSpy.Server.PresenceSearchPlayer.Exception.NewUser
{
    public class GPNewUserUniquenickInvalidException : GPNewUserException
    {
        public GPNewUserUniquenickInvalidException() : base("Uniquenick is invalid!", GPErrorCode.NewUserUniquenickInvalid)
        {
        }

        public GPNewUserUniquenickInvalidException(string message) : base(message, GPErrorCode.NewUserUniquenickInvalid)
        {
        }

        public GPNewUserUniquenickInvalidException(string message, System.Exception innerException) : base(message, GPErrorCode.NewUserUniquenickInvalid, innerException)
        {
        }
    }
}