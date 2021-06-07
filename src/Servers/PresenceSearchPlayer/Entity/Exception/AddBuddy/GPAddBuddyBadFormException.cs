using System;
using PresenceSearchPlayer.Entity.Exception.General;
using PresenceSearchPlayer.Entity.Enumerate;

namespace PresenceSearchPlayer.Entity.Exception.AddBuddy
{
    public class GPAddBuddyBadFormException : GPException
    {
        public GPAddBuddyBadFormException() : base("Add buddy format invalid!", GPErrorCode.AddBuddyBadForm)
        {
        }

        public GPAddBuddyBadFormException(string message) : base(message, GPErrorCode.AddBuddyBadForm)
        {
        }

        public GPAddBuddyBadFormException(string message, System.Exception innerException) : base(message, GPErrorCode.AddBuddyBadForm, innerException)
        {
        }
    }
}