using UniSpyServer.Servers.PresenceSearchPlayer.Entity.Enumerate;
using UniSpyServer.Servers.PresenceSearchPlayer.Entity.Exception.General;

namespace UniSpyServer.Servers.PresenceSearchPlayer.Entity.Exception.BM
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