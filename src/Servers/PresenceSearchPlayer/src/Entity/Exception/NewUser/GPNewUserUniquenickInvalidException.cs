using UniSpy.Server.PresenceSearchPlayer.Entity.Enumerate;

namespace UniSpy.Server.PresenceSearchPlayer.Entity.Exception.NewUser
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