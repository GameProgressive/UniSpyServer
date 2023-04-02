namespace UniSpy.Server.WebServer.Module.Auth
{
    public class ServerException : Auth.Exception
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