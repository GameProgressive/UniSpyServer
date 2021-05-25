using System;
using PresenceSearchPlayer.Abstraction.BaseClass;
using PresenceSearchPlayer.Entity.Enumerate;

namespace PresenceSearchPlayer.Entity.Exceptions.NewProfile
{
    public class GPNewProfileBadNickException : GPExceptionBase
    {
        public GPNewProfileBadNickException() : base("Nickname is invalid at creating new profile!")
        {
            ErrorCode = GPErrorCode.NewProfileBadNick;
        }

        public GPNewProfileBadNickException(string message) : base(message)
        {
            ErrorCode = GPErrorCode.NewProfileBadNick;
        }

        public GPNewProfileBadNickException(string message, Exception innerException) : base(message, innerException)
        {
            ErrorCode = GPErrorCode.NewProfileBadNick;
        }
    }
}