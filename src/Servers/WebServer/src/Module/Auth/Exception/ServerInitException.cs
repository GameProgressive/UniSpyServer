namespace UniSpy.Server.WebServer.Module.Auth
{
    public class ServerInitException : Auth.Exception
    {
        public ServerInitException() : base("An unknown error occur when initializing server!", AuthErrorCode.ServerError)
        {
        }
        public ServerInitException(string message) : base(message, AuthErrorCode.ServerError)
        {
        }
        public ServerInitException(string message, System.Exception innerException) : base(message, AuthErrorCode.ServerError, innerException)
        {
        }
    }
}