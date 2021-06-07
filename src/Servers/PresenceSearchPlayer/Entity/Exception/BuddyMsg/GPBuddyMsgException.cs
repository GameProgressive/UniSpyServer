using System;
using PresenceSearchPlayer.Abstraction.BaseClass;
using PresenceSearchPlayer.Entity.Enumerate;
using PresenceSearchPlayer.Entity.Exception.General;

namespace PresenceSearchPlayer.Entity.Exception.BM
{
    public class GPBuddyMsgException : GPException
    {
        public GPBuddyMsgException() : base("Unknown error occur when processing buddy message!", GPErrorCode.Bm)
        {
        }

        public GPBuddyMsgException(string message) : base(message, GPErrorCode.Bm)
        {
        }

        public GPBuddyMsgException(string message, System.Exception innerException) : base(message, GPErrorCode.Bm, innerException)
        {
        }
    }
}