using UniSpyServer.PresenceSearchPlayer.Entity.Enumerate;

namespace UniSpyServer.PresenceSearchPlayer.Entity.Exception.Login
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