using PresenceSearchPlayer.Abstraction.BaseClass;
using PresenceSearchPlayer.Entity.Enumerate;

namespace PresenceSearchPlayer.Entity.Exceptions.General
{
    public class GPConnectionCloseException : GPExceptionBase
    {
        public GPConnectionCloseException() : base("Client connection accidently closed!")
        {
            ErrorCode = GPErrorCode.ConnectionClose;
        }

        public GPConnectionCloseException(string message) : base(message)
        {
            ErrorCode = GPErrorCode.ConnectionClose;
        }

        public GPConnectionCloseException(string message, System.Exception innerException) : base(message, innerException)
        {
            ErrorCode = GPErrorCode.ConnectionClose;
        }
    }
}