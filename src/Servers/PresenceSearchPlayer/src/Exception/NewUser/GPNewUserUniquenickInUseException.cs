using UniSpy.Server.PresenceSearchPlayer.Enumerate;

namespace UniSpy.Server.PresenceSearchPlayer.Exception.NewUser
{
    public class GPNewUserUniquenickInUseException : GPNewUserException
    {
        public GPNewUserUniquenickInUseException() : base("Uniquenick is in use!", GPErrorCode.NewUserUniquenickInUse)
        {
        }

        public GPNewUserUniquenickInUseException(string message) : base(message, GPErrorCode.NewUserUniquenickInUse)
        {
        }

        public GPNewUserUniquenickInUseException(string message, System.Exception innerException) : base(message, GPErrorCode.NewUserUniquenickInUse, innerException)
        {
        }
    }
}