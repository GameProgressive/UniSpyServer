using System;
using PresenceSearchPlayer.Abstraction.BaseClass;
using PresenceSearchPlayer.Entity.Enumerate;

namespace PresenceSearchPlayer.Entity.Exceptions
{
    public class GPBuddyMsgExtInfoNotSupportedException : GPExceptionBase
    {
        public GPBuddyMsgExtInfoNotSupportedException()
        {
            ErrorCode = GPErrorCode.BmExtInfoNotSupported;
        }

        public GPBuddyMsgExtInfoNotSupportedException(string message) : base(message)
        {
            ErrorCode = GPErrorCode.BmExtInfoNotSupported;
        }

        public GPBuddyMsgExtInfoNotSupportedException(string message, Exception innerException) : base(message, innerException)
        {
            ErrorCode = GPErrorCode.BmExtInfoNotSupported;
        }
    }
}