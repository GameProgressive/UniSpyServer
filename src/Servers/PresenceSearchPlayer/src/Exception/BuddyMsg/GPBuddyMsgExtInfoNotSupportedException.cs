using UniSpy.Server.PresenceSearchPlayer.Enumerate;
using UniSpy.Server.PresenceSearchPlayer.Exception.General;

namespace UniSpy.Server.PresenceSearchPlayer.Exception
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