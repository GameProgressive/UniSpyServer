using UniSpy.Server.PresenceSearchPlayer.Enumerate;

namespace UniSpy.Server.PresenceSearchPlayer.Exception.NewUser
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