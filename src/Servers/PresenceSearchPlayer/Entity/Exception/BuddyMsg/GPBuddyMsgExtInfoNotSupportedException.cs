using System;
using PresenceSearchPlayer.Abstraction.BaseClass;
using PresenceSearchPlayer.Entity.Enumerate;
using PresenceSearchPlayer.Entity.Exception.General;

namespace PresenceSearchPlayer.Entity.Exception
{
    public class GPBuddyMsgExtInfoNotSupportedException : GPException
    {
        public GPBuddyMsgExtInfoNotSupportedException() : base("Buddy message is not supported.", GPErrorCode.BmExtInfoNotSupported)
        {
        }

        public GPBuddyMsgExtInfoNotSupportedException(string message) : base(message, GPErrorCode.BmExtInfoNotSupported)
        {
        }

        public GPBuddyMsgExtInfoNotSupportedException(string message, System.Exception innerException) : base(message, GPErrorCode.BmExtInfoNotSupported, innerException)
        {
        }
    }
}