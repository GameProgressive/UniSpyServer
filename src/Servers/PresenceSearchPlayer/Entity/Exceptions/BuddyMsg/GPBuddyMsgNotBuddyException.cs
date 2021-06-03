using System;
using PresenceSearchPlayer.Abstraction.BaseClass;
using PresenceSearchPlayer.Entity.Enumerate;

namespace PresenceSearchPlayer.Entity.Exceptions.BM
{
    public class GPBuddyMsgNotBuddyException : GPExceptionBase
    {
        public GPBuddyMsgNotBuddyException() : base("The message receiver is not your buddy!")
        {
            ErrorCode = GPErrorCode.BmNotBuddy;
        }

        public GPBuddyMsgNotBuddyException(string message) : base(message)
        {
            ErrorCode = GPErrorCode.BmNotBuddy;
        }

        public GPBuddyMsgNotBuddyException(string message, Exception innerException) : base(message, innerException)
        {
            ErrorCode = GPErrorCode.BmNotBuddy;
        }
    }
}