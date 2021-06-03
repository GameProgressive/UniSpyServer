using System;
using PresenceSearchPlayer.Abstraction.BaseClass;
using PresenceSearchPlayer.Entity.Enumerate;

namespace PresenceSearchPlayer.Entity.Exceptions.BM
{
    public class GPBuddyMsgException : GPExceptionBase
    {
        public GPBuddyMsgException():base("Unknown error occur when processing buddy message!")
        {
            ErrorCode = GPErrorCode.Bm;
        }

        public GPBuddyMsgException(string message) : base(message)
        {
            ErrorCode = GPErrorCode.Bm;
        }

        public GPBuddyMsgException(string message, Exception innerException) : base(message, innerException)
        {
            ErrorCode = GPErrorCode.Bm;
        }
    }
}