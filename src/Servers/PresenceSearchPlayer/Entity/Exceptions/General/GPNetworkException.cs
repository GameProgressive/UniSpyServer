using PresenceSearchPlayer.Abstraction.BaseClass;
using PresenceSearchPlayer.Entity.Enumerate;

namespace PresenceSearchPlayer.Entity.Exceptions.General
{
    public class GPNetworkException : GPExceptionBase
    {
        public GPNetworkException() : base("Unknown network error!")
        {
            ErrorCode = GPErrorCode.Network;
        }

        public GPNetworkException(string message) : base(message)
        {
            ErrorCode = GPErrorCode.Network;
        }

        public GPNetworkException(string message, System.Exception innerException) : base(message, innerException)
        {
            ErrorCode = GPErrorCode.Network;
        }
    }
}