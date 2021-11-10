using UniSpyServer.Servers.PresenceSearchPlayer.Entity.Enumerate;

namespace UniSpyServer.Servers.PresenceSearchPlayer.Entity.Exception.Login
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
