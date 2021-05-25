using System;
using PresenceSearchPlayer.Abstraction.BaseClass;
using PresenceSearchPlayer.Entity.Enumerate;

namespace PresenceSearchPlayer.Entity.Exceptions.NewUser
{
    public class GPNewUserBadNickException : GPExceptionBase
    {
        public GPNewUserBadNickException() : base("The nickname provided is invalid!")
        {
            ErrorCode = GPErrorCode.NewUserBadNick;
        }

        public GPNewUserBadNickException(string message) : base(message)
        {
            ErrorCode = GPErrorCode.NewUserBadNick;
        }

        public GPNewUserBadNickException(string message, Exception innerException) : base(message, innerException)
        {
            ErrorCode = GPErrorCode.NewUserBadNick;
        }
    }
}