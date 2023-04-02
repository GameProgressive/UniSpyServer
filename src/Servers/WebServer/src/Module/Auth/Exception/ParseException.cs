namespace UniSpy.Server.WebServer.Module.Auth
{
    public class ParseException : Auth.Exception
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