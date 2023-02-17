using UniSpy.Server.PresenceSearchPlayer.Enumerate;

namespace UniSpy.Server.PresenceSearchPlayer.Exception.Login
{
    public class GPLoginTimeOutException : GPLoginException
    {
        public GPLoginTimeOutException() : base("Login timeout!", GPErrorCode.LoginTimeOut)
        {
        }

        public GPLoginTimeOutException(string message) : base(message, GPErrorCode.LoginTimeOut)
        {
        }

        public GPLoginTimeOutException(string message, System.Exception innerException) : base(message, GPErrorCode.LoginTimeOut, innerException)
        {
        }
    }
}
