namespace UniSpyServer.Servers.WebServer.Module.Auth.Exception
{
    public class InvalidPasswordException : AuthException
    {
        public InvalidPasswordException() : base("Password is invalid!", AuthErrorCode.InvalidPassword)
        {
        }
        public InvalidPasswordException(string message) : base(message, AuthErrorCode.InvalidPassword)
        {
        }
        public InvalidPasswordException(string message, System.Exception innerException) : base(message, AuthErrorCode.InvalidPassword, innerException)
        {
        }
    }
}