using System;
using PresenceSearchPlayer.Abstraction.BaseClass;
using PresenceSearchPlayer.Entity.Enumerate;

namespace PresenceSearchPlayer.Entity.Exceptions.UpdatePro
{
    public class GPUpdateProBadNickException : GPExceptionBase
    {
        public GPUpdateProBadNickException() : base("Nickname is invalid for updating profile!")
        {
            ErrorCode = GPErrorCode.UpdateProBadNick;
        }

        public GPUpdateProBadNickException(string message) : base(message)
        {
            ErrorCode = GPErrorCode.UpdateProBadNick;
        }

        public GPUpdateProBadNickException(string message, Exception innerException) : base(message, innerException)
        {
            ErrorCode = GPErrorCode.UpdateProBadNick;
        }
    }
}