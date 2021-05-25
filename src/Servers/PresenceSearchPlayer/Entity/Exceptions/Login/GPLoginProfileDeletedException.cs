using System;
using PresenceSearchPlayer.Abstraction.BaseClass;
using PresenceSearchPlayer.Entity.Enumerate;

namespace PresenceSearchPlayer.Entity.Exceptions.Login
{
    public class GPLoginProfileDeletedException : GPExceptionBase
    {
        public GPLoginProfileDeletedException() : base("User's profile has been deleted!")
        {
            ErrorCode = GPErrorCode.LoginProfileDeleted;
        }

        public GPLoginProfileDeletedException(string message) : base(message)
        {
            ErrorCode = GPErrorCode.LoginProfileDeleted;
        }

        public GPLoginProfileDeletedException(string message, Exception innerException) : base(message, innerException)
        {
            ErrorCode = GPErrorCode.LoginProfileDeleted;
        }
    }
}