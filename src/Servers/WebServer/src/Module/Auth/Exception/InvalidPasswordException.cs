namespace UniSpy.Server.WebServer.Module.Auth
{
    public class InvalidPasswordException : Auth.Exception
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