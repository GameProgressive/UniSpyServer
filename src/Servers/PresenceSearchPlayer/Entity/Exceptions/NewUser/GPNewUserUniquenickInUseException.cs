using System;
using PresenceSearchPlayer.Abstraction.BaseClass;
using PresenceSearchPlayer.Entity.Enumerate;

namespace PresenceSearchPlayer.Entity.Exceptions.NewUser
{
    public class GPNewUserUniquenickInUseException : GPExceptionBase
    {
        public GPNewUserUniquenickInUseException() : base("Uniquenick is in use!")
        {
            ErrorCode = GPErrorCode.NewUserUniquenickInUse;
        }

        public GPNewUserUniquenickInUseException(string message) : base(message)
        {
            ErrorCode = GPErrorCode.NewUserUniquenickInUse;
        }

        public GPNewUserUniquenickInUseException(string message, Exception innerException) : base(message, innerException)
        {
            ErrorCode = GPErrorCode.NewUserUniquenickInUse;
        }
    }
}