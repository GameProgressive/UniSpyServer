using System;
using PresenceSearchPlayer.Abstraction.BaseClass;
using PresenceSearchPlayer.Entity.Enumerate;

namespace PresenceSearchPlayer.Entity.Exceptions.NewUser
{
    public class GPNewUserBadPasswordException : GPExceptionBase
    {
        public GPNewUserBadPasswordException() : base("Password is invalid!")
        {
            ErrorCode = GPErrorCode.NewUserBadPasswords;
        }

        public GPNewUserBadPasswordException(string message) : base(message)
        {
            ErrorCode = GPErrorCode.NewUserBadPasswords;
        }

        public GPNewUserBadPasswordException(string message, Exception innerException) : base(message, innerException)
        {
            ErrorCode = GPErrorCode.NewUserBadPasswords;
        }
    }
}