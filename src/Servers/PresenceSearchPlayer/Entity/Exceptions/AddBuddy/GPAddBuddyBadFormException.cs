using System;
using PresenceSearchPlayer.Abstraction.BaseClass;
using PresenceSearchPlayer.Entity.Enumerate;

namespace PresenceSearchPlayer.Entity.Exceptions.AddBuddy
{
    public class GPAddBuddyBadFormException : GPExceptionBase
    {
        public GPAddBuddyBadFormException() : base("Add buddy format invalid!")
        {
            ErrorCode = GPErrorCode.AddBuddyBadForm;
        }

        public GPAddBuddyBadFormException(string message) : base(message)
        {
            ErrorCode = GPErrorCode.AddBuddyBadForm;
        }

        public GPAddBuddyBadFormException(string message, Exception innerException) : base(message, innerException)
        {
            ErrorCode = GPErrorCode.AddBuddyBadForm;
        }
    }
}