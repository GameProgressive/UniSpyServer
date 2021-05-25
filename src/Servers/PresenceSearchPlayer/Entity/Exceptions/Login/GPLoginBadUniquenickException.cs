using System;
using PresenceSearchPlayer.Abstraction.BaseClass;
using PresenceSearchPlayer.Entity.Enumerate;

namespace PresenceSearchPlayer.Entity.Exceptions.Login
{
    public class GPLoginBadUniquenickException : GPExceptionBase
    {
        public GPLoginBadUniquenickException() : base("The uniquenick provided is invalid!")
        {
            ErrorCode = GPErrorCode.LoginBadUniquenick;
        }

        public GPLoginBadUniquenickException(string message) : base(message)
        {
            ErrorCode = GPErrorCode.LoginBadUniquenick;
        }

        public GPLoginBadUniquenickException(string message, Exception innerException) : base(message, innerException)
        {
            ErrorCode = GPErrorCode.LoginBadUniquenick;
        }
    }
}