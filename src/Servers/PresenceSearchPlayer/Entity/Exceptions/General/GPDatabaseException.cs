using System;
using PresenceSearchPlayer.Abstraction.BaseClass;
using PresenceSearchPlayer.Entity.Enumerate;

namespace PresenceSearchPlayer.Entity.Exceptions.General
{
    public class GPDatabaseException : GPExceptionBase
    {
        public GPDatabaseException() : base("Database error!")
        {
            ErrorCode = GPErrorCode.DatabaseError;
        }

        public GPDatabaseException(string message) : base(message)
        {
            ErrorCode = GPErrorCode.DatabaseError;
        }

        public GPDatabaseException(string message, Exception innerException) : base(message, innerException)
        {
            ErrorCode = GPErrorCode.DatabaseError;
        }
    }
}