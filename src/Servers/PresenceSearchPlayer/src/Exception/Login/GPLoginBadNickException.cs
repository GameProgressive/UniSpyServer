using UniSpy.Server.PresenceSearchPlayer.Enumerate;

namespace UniSpy.Server.PresenceSearchPlayer.Exception.Login
{
    public class GPLoginBadNickException : GPLoginException
    {
        public GPLoginBadNickException() : base("Nickname is in valid!", GPErrorCode.LoginBadNick)
        {
        }

        public GPLoginBadNickException(string message) : base(message, GPErrorCode.LoginBadNick)
        {
        }

        public GPLoginBadNickException(string message, System.Exception innerException) : base(message, GPErrorCode.LoginBadNick, innerException)
        {
        }
    }
}