namespace UniSpyServer.Servers.WebServer.Module.Auth.Exception
{
    public class ParseException : AuthException
    {
        public ParseException() : base("Parse error!", AuthErrorCode.ParseError)
        {
        }
        public ParseException(string message) : base(message, AuthErrorCode.ParseError)
        {
        }
        public ParseException(string message, System.Exception innerException) : base(message, AuthErrorCode.ParseError, innerException)
        {
        }
    }
}