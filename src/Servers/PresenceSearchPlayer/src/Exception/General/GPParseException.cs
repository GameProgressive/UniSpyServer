using UniSpy.Server.PresenceSearchPlayer.Enumerate;

namespace UniSpy.Server.PresenceSearchPlayer.Exception.General
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