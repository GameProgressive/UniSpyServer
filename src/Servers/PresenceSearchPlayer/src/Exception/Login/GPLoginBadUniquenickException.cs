using UniSpy.Server.PresenceSearchPlayer.Enumerate;

namespace UniSpy.Server.PresenceSearchPlayer.Exception.Login
{
    public class GPLoginBadUniquenickException : GPLoginException
    {
        public GPLoginBadUniquenickException() : base("The uniquenick provided is invalid!", GPErrorCode.LoginBadUniquenick)
        {
        }

        public GPLoginBadUniquenickException(string message) : base(message, GPErrorCode.LoginBadUniquenick)
        {
        }

        public GPLoginBadUniquenickException(string message, System.Exception innerException) : base(message, GPErrorCode.LoginBadUniquenick, innerException)
        {
        }
    }
}