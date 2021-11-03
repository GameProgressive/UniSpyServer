using UniSpyServer.Servers.PresenceSearchPlayer.Entity.Enumerate;
using UniSpyServer.Servers.PresenceSearchPlayer.Entity.Exception.General;

namespace UniSpyServer.Servers.PresenceSearchPlayer.Entity.Exception.UpdatePro
{
    public class GPUpdateProException : GPException
    {
        public GPUpdateProException() : base("Update profile unknown error!", GPErrorCode.UpdatePro)
        {
        }

        public GPUpdateProException(string message) : base(message, GPErrorCode.UpdatePro)
        {
        }

        public GPUpdateProException(string message, System.Exception innerException) : base(message, GPErrorCode.UpdatePro, innerException)
        {
        }

        public GPUpdateProException(string message, GPErrorCode errorCode) : base(message, errorCode)
        {
        }

        public GPUpdateProException(string message, GPErrorCode errorCode, System.Exception innerException) : base(message, errorCode, innerException)
        {
        }
    }
}