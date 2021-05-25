using System;
using PresenceSearchPlayer.Abstraction.BaseClass;
using PresenceSearchPlayer.Entity.Enumerate;

namespace PresenceSearchPlayer.Entity.Exceptions.Login
{
    public class GPLoginBadPasswordException : GPExceptionBase
    {
        public GPLoginBadPasswordException() : base("Password provided is invalid!")
        {
            ErrorCode = GPErrorCode.LoginBadPassword;
        }

        public GPLoginBadPasswordException(string message) : base(message)
        {
            ErrorCode = GPErrorCode.LoginBadPassword;
        }

        public GPLoginBadPasswordException(string message, Exception innerException) : base(message, innerException)
        {
            ErrorCode = GPErrorCode.LoginBadPassword;
        }
    }
}