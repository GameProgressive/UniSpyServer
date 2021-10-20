using UniSpyServer.PresenceSearchPlayer.Entity.Enumerate;
using UniSpyServer.PresenceSearchPlayer.Entity.Exception.General;

namespace UniSpyServer.PresenceSearchPlayer.Entity.Exception.Login
{
    public class GPLoginException : GPException
    {
        public GPLoginException() : base("Unknown login error!", GPErrorCode.Login)
        {
        }

        public GPLoginException(string message) : base(message, GPErrorCode.Login)
        {
        }

        public GPLoginException(string message, System.Exception innerException) : base(message, GPErrorCode.Login, innerException)
        {
        }

        public GPLoginException(string message, GPErrorCode errorCode) : base(message, errorCode)
        {
        }

        public GPLoginException(string message, GPErrorCode errorCode, System.Exception innerException) : base(message, errorCode, innerException)
        {
        }
    }
}