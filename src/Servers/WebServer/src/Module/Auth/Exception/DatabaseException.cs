namespace UniSpy.Server.WebServer.Module.Auth.Exception
{
    public class DatabaseException:AuthException
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