using System;
using PresenceSearchPlayer.Abstraction.BaseClass;
using PresenceSearchPlayer.Entity.Enumerate;

namespace PresenceSearchPlayer.Entity.Exceptions.Login
{
    public class GPLoginConnectionFailedException : GPExceptionBase
    {
        public GPLoginConnectionFailedException() : base("Login connection failed!")
        {
            ErrorCode = GPErrorCode.LoginConnectionFailed;
        }

        public GPLoginConnectionFailedException(string message) : base(message)
        {
            ErrorCode = GPErrorCode.LoginConnectionFailed;
        }

        public GPLoginConnectionFailedException(string message, Exception innerException) : base(message, innerException)
        {
            ErrorCode = GPErrorCode.LoginConnectionFailed;
        }
    }
}