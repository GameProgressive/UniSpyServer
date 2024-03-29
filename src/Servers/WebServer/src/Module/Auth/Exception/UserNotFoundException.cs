namespace UniSpy.Server.WebServer.Module.Auth
{
    public class UserNotFoundException : Auth.Exception
    {
        public UserNotFoundException() : base("User not found!", AuthErrorCode.UserNotFound)
        {
        }
        public UserNotFoundException(string message) : base(message, AuthErrorCode.UserNotFound)
        {
        }
        public UserNotFoundException(string message, System.Exception innerException) : base(message, AuthErrorCode.UserNotFound, innerException)
        {
        }
    }
}