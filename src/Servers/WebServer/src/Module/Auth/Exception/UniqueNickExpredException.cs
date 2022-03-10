namespace UniSpyServer.Servers.WebServer.Module.Auth.Exception
{
    public class UniqueNickExpredException : AuthException
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