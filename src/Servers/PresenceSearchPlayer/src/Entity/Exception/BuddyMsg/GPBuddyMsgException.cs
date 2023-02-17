using UniSpy.Server.PresenceSearchPlayer.Entity.Enumerate;
using UniSpy.Server.PresenceSearchPlayer.Entity.Exception.General;

namespace UniSpy.Server.PresenceSearchPlayer.Entity.Exception.BM
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