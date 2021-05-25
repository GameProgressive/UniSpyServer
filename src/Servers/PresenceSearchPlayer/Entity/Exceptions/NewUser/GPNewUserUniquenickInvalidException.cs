using System;
using PresenceSearchPlayer.Abstraction.BaseClass;
using PresenceSearchPlayer.Entity.Enumerate;

namespace PresenceSearchPlayer.Entity.Exceptions.NewUser
{
    public class GPNewUserUniquenickInvalidException : GPExceptionBase
    {
        public GPNewUserUniquenickInvalidException() : base("Uniquenick is invalid!")
        {
            ErrorCode = GPErrorCode.NewUserUniquenickInvalid;
        }

        public GPNewUserUniquenickInvalidException(string message) : base(message)
        {
            ErrorCode = GPErrorCode.NewUserUniquenickInvalid;
        }

        public GPNewUserUniquenickInvalidException(string message, Exception innerException) : base(message, innerException)
        {
            ErrorCode = GPErrorCode.NewUserUniquenickInvalid;
        }
    }
}