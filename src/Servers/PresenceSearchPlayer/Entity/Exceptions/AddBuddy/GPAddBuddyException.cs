using System;
using PresenceSearchPlayer.Abstraction.BaseClass;
using PresenceSearchPlayer.Entity.Enumerate;

namespace PresenceSearchPlayer.Entity.Exceptions.AddBuddy
{
    public class GPAddBuddyException : GPExceptionBase
    {
        public GPAddBuddyException() : base("Unknown error occur at add buddy!")
        {
            ErrorCode = GPErrorCode.AddBuddy;
        }

        public GPAddBuddyException(string message) : base(message)
        {
            ErrorCode = GPErrorCode.AddBuddy;
        }

        public GPAddBuddyException(string message, Exception innerException) : base(message, innerException)
        {
            ErrorCode = GPErrorCode.AddBuddy;
        }
    }
}