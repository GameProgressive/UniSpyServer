using System;
using PresenceSearchPlayer.Abstraction.BaseClass;
using PresenceSearchPlayer.Entity.Enumerate;

namespace PresenceSearchPlayer.Entity.Exceptions.Login
{
    public class GPLoginBadProfileException : GPExceptionBase
    {
        public GPLoginBadProfileException() : base("User profile is damaged!")
        {
            ErrorCode = GPErrorCode.LoginBadProfile;
        }

        public GPLoginBadProfileException(string message) : base(message)
        {
            ErrorCode = GPErrorCode.LoginBadProfile;
        }

        public GPLoginBadProfileException(string message, Exception innerException) : base(message, innerException)
        {
            ErrorCode = GPErrorCode.LoginBadProfile;
        }
    }
}