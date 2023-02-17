using UniSpy.Server.PresenceSearchPlayer.Enumerate;
using UniSpy.Server.PresenceSearchPlayer.Exception.General;

namespace UniSpy.Server.PresenceSearchPlayer.Exception.BM
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