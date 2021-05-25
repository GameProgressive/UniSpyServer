using System;
using PresenceSearchPlayer.Abstraction.BaseClass;
using PresenceSearchPlayer.Entity.Enumerate;

namespace PresenceSearchPlayer.Entity.Exceptions.NewProfile
{
    public class GPNewProfileException : GPExceptionBase
    {
        public GPNewProfileException() : base("An unknown error occur when creating new profile!")
        {
            ErrorCode = GPErrorCode.NewProfile;
        }

        public GPNewProfileException(string message) : base(message)
        {
            ErrorCode = GPErrorCode.NewProfile;
        }

        public GPNewProfileException(string message, Exception innerException) : base(message, innerException)
        {
            ErrorCode = GPErrorCode.NewProfile;
        }
    }
}