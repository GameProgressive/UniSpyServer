using UniSpy.Server.PresenceSearchPlayer.Enumerate;
using UniSpy.Server.PresenceSearchPlayer.Exception.General;

namespace UniSpy.Server.PresenceSearchPlayer.Exception.BM
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