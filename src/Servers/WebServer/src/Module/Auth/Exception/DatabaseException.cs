namespace UniSpy.Server.WebServer.Module.Auth
{
    public class DatabaseException : Auth.Exception
    {
        public DatabaseException() : base("Database error!", AuthErrorCode.DatabaseError)
        {
        }
        public DatabaseException(string message) : base(message, AuthErrorCode.DatabaseError)
        {
        }
        public DatabaseException(string message, System.Exception innerException) : base(message, AuthErrorCode.DatabaseError, innerException)
        {
        }
    }
}