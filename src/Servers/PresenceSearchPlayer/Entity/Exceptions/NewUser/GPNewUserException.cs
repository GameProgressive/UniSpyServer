using System;
using PresenceSearchPlayer.Abstraction.BaseClass;
using PresenceSearchPlayer.Entity.Enumerate;

namespace PresenceSearchPlayer.Entity.Exceptions.NewUser
{
    public class GPNewUserException : GPExceptionBase
    {
        public GPNewUserException() : base("Create new user unknown error!")
        {
            ErrorCode = GPErrorCode.NewUser;
        }

        public GPNewUserException(string message) : base(message)
        {
            ErrorCode = GPErrorCode.NewUser;
        }

        public GPNewUserException(string message, Exception innerException) : base(message, innerException)
        {
            ErrorCode = GPErrorCode.NewUser;
        }
    }
}