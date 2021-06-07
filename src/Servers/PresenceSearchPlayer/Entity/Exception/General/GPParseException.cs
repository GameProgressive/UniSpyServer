using PresenceSearchPlayer.Abstraction.BaseClass;
using PresenceSearchPlayer.Entity.Enumerate;

namespace PresenceSearchPlayer.Entity.Exception.General
{
    public class GPParseException : GPException
    {
        public GPParseException() : base("Request parsing error!", GPErrorCode.Parse)
        {
        }

        public GPParseException(string message) : base(message, GPErrorCode.Parse)
        {
        }

        public GPParseException(string message, System.Exception innerException) : base(message, GPErrorCode.Parse, innerException)
        {
        }
    }
}