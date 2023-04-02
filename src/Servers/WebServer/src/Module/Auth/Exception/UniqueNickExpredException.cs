namespace UniSpy.Server.WebServer.Module.Auth
{
    public class UniqueNickExpredException : Auth.Exception
    {
        public UniqueNickExpredException() : base("Nickname is invalid at creating new profile!", AuthErrorCode.UniqueNickExpired)
        {
        }
        public UniqueNickExpredException(string message) : base(message, AuthErrorCode.UniqueNickExpired)
        {
        }
        public UniqueNickExpredException(string message, System.Exception innerException) : base(message, AuthErrorCode.UniqueNickExpired, innerException)
        {
        }
    }
}