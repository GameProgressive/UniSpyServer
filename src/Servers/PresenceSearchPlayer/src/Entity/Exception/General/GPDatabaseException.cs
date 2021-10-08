using PresenceSearchPlayer.Entity.Enumerate;

namespace PresenceSearchPlayer.Entity.Exception.General
{
    public class GPDatabaseException : GPException
    {
        public GPDatabaseException() : base("Database error!", GPErrorCode.DatabaseError)
        {
        }

        public GPDatabaseException(string message) : base(message, GPErrorCode.DatabaseError)
        {
        }

        public GPDatabaseException(string message, System.Exception innerException) : base(message, GPErrorCode.DatabaseError, innerException)
        {
        }
    }
}