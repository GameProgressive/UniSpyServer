using PresenceSearchPlayer.Abstraction.BaseClass;
using PresenceSearchPlayer.Entity.Enumerate;

namespace PresenceSearchPlayer.Entity.Exceptions.General
{
    public class GPForcedDisconnectException : GPExceptionBase
    {
        public GPForcedDisconnectException() : base("Client is forced to disconnect!")
        {
            ErrorCode = GPErrorCode.ForcedDisconnect;
        }

        public GPForcedDisconnectException(string message) : base(message)
        {
            ErrorCode = GPErrorCode.ForcedDisconnect;

        }

        public GPForcedDisconnectException(string message, System.Exception innerException) : base(message, innerException)
        {
            ErrorCode = GPErrorCode.ForcedDisconnect;

        }
    }
}