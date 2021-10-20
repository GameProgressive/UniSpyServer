using UniSpyServer.PresenceSearchPlayer.Entity.Enumerate;

namespace UniSpyServer.PresenceSearchPlayer.Entity.Exception.NewUser
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