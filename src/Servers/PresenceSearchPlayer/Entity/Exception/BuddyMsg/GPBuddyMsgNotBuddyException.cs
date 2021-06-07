using PresenceSearchPlayer.Entity.Enumerate;
using PresenceSearchPlayer.Entity.Exception.General;

namespace PresenceSearchPlayer.Entity.Exception.BM
{
    public class GPBuddyMsgNotBuddyException : GPException
    {
        public GPBuddyMsgNotBuddyException() : base("The message receiver is not your buddy!", GPErrorCode.BmNotBuddy)
        {
        }

        public GPBuddyMsgNotBuddyException(string message) : base(message, GPErrorCode.BmNotBuddy)
        {
        }

        public GPBuddyMsgNotBuddyException(string message, System.Exception innerException) : base(message, GPErrorCode.BmNotBuddy, innerException)
        {
        }
    }
}