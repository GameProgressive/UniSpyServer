namespace UniSpy.Server.WebServer.Module.Auth.Exception
{
    public class InvalidProfileException : AuthException
    {
        public InvalidProfileException() : base("Profile is invalid!", AuthErrorCode.InvalidProfile)
        {
        }
        public InvalidProfileException(string message) : base(message, AuthErrorCode.InvalidProfile)
        {
        }
        public InvalidProfileException(string message, System.Exception innerException) : base(message, AuthErrorCode.InvalidProfile, innerException)
        {
        }
    }
}