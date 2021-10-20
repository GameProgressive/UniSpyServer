using UniSpyServer.PresenceSearchPlayer.Entity.Enumerate;
using UniSpyServer.PresenceSearchPlayer.Entity.Exception.General;

namespace UniSpyServer.PresenceSearchPlayer.Entity.Exception.BM
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