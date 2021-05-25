using PresenceSearchPlayer.Abstraction.BaseClass;
using PresenceSearchPlayer.Entity.Enumerate;

namespace PresenceSearchPlayer.Entity.Exceptions.General
{
    public class GPParseException : GPExceptionBase
    {
        public GPParseException() : base("Parsing error!")
        {
            ErrorCode = GPErrorCode.Parse;
        }

        public GPParseException(string message) : base(message)
        {
            ErrorCode = GPErrorCode.Parse;
        }

        public GPParseException(string message, System.Exception innerException) : base(message, innerException)
        {
            ErrorCode = GPErrorCode.Parse;
        }
    }
}