using UniSpyServer.Servers.PresenceSearchPlayer.Entity.Enumerate;

namespace UniSpyServer.Servers.PresenceSearchPlayer.Entity.Exception.General
{
    public class GPBadSessionKeyException : GPException
    {
        public GPBadSessionKeyException() : base("Your session key is not valid!", GPErrorCode.BadSessionKey)
        {
        }

        public GPBadSessionKeyException(string message) : base(message, GPErrorCode.BadSessionKey)
        {
        }

        public GPBadSessionKeyException(string message, System.Exception innerException) : base(message, GPErrorCode.BadSessionKey, innerException)
        {
        }
    }
}
