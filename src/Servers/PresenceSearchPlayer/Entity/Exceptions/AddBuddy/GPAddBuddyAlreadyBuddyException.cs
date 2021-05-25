using System;
using PresenceSearchPlayer.Abstraction.BaseClass;
using PresenceSearchPlayer.Entity.Enumerate;

namespace PresenceSearchPlayer.Entity.Exceptions.AddBuddy
{
    public class GPAddBuddyAlreadyBuddyException : GPExceptionBase
    {
        public GPAddBuddyAlreadyBuddyException() : base("The buddy you are adding is already in your buddy list!")
        {
            ErrorCode = GPErrorCode.AddBuddyAlreadyBuddy;
        }

        public GPAddBuddyAlreadyBuddyException(string message) : base(message)
        {
            ErrorCode = GPErrorCode.AddBuddyAlreadyBuddy;
        }

        public GPAddBuddyAlreadyBuddyException(string message, Exception innerException) : base(message, innerException)
        {
            ErrorCode = GPErrorCode.AddBuddyAlreadyBuddy;
        }
    }
}