using UniSpy.Server.PresenceSearchPlayer.Enumerate;

namespace UniSpy.Server.PresenceSearchPlayer.Exception.General
{
    public class GPBadSessionKeyException : GPException
    {
        public GPBadSessionKeyException() : base("Your connection key is not valid!", GPErrorCode.BadSessionKey)
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
