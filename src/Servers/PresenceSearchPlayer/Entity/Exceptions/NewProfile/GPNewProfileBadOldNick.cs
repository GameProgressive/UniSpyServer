using System;
using PresenceSearchPlayer.Abstraction.BaseClass;
using PresenceSearchPlayer.Entity.Enumerate;

namespace PresenceSearchPlayer.Entity.Exceptions.NewProfile
{
    public class GPNewProfileBadOldNickException : GPExceptionBase
    {
        public GPNewProfileBadOldNickException() : base("There is an already exist nickname!")
        {
            ErrorCode = GPErrorCode.NewProfileBadOldNick;
        }

        public GPNewProfileBadOldNickException(string message) : base(message)
        {
            ErrorCode = GPErrorCode.NewProfileBadOldNick;
        }

        public GPNewProfileBadOldNickException(string message, Exception innerException) : base(message, innerException)
        {
            ErrorCode = GPErrorCode.NewProfileBadOldNick;
        }
    }
}