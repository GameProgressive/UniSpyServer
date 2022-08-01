namespace UniSpyServer.Servers.WebServer.Module.Auth.Exception
{
    public class ServerException : AuthException
    {
        public ServerException() : base("Server error!", AuthErrorCode.ServerError)
        {
        }
        public ServerException(string message) : base(message, AuthErrorCode.ServerError)
        {
        }
        public ServerException(string message, System.Exception innerException) : base(message, AuthErrorCode.ServerError, innerException)
        {
        }
    }
}