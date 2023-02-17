using UniSpy.Server.PresenceSearchPlayer.Entity.Enumerate;

namespace UniSpy.Server.PresenceSearchPlayer.Entity.Exception.NewUser
{
    public class GPNewUserBadNickException : GPNewUserException
    {
        public GPNewUserBadNickException() : base("The nickname provided is invalid!", GPErrorCode.NewUserBadNick)
        {
        }

        public GPNewUserBadNickException(string message) : base(message, GPErrorCode.NewUserBadNick)
        {
        }

        public GPNewUserBadNickException(string message, System.Exception innerException) : base(message, GPErrorCode.NewUserBadNick, innerException)
        {
        }
    }
}