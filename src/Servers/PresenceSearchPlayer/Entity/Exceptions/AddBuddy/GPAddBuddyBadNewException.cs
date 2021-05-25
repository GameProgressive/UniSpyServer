using System;
using PresenceSearchPlayer.Abstraction.BaseClass;
using PresenceSearchPlayer.Entity.Enumerate;

namespace PresenceSearchPlayer.Entity.Exceptions.AddBuddy
{
    public class GPAddBuddyBadNewException : GPExceptionBase
    {
        public GPAddBuddyBadNewException() : base("The buddy name provided is invalid!")
        {
            ErrorCode = GPErrorCode.AddBuddyBadNew;
        }

        public GPAddBuddyBadNewException(string message) : base(message)
        {
            ErrorCode = GPErrorCode.AddBuddyBadNew;
        }

        public GPAddBuddyBadNewException(string message, Exception innerException) : base(message, innerException)
        {
            ErrorCode = GPErrorCode.AddBuddyBadNew;
        }
    }
}