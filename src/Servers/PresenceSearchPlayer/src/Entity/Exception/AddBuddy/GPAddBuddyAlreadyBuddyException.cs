using UniSpyServer.Servers.PresenceSearchPlayer.Entity.Enumerate;
using UniSpyServer.Servers.PresenceSearchPlayer.Entity.Exception.General;

namespace UniSpyServer.Servers.PresenceSearchPlayer.Entity.Exception.AddBuddy
{
    public class GPAddBuddyAlreadyBuddyException : GPException
    {
        public GPAddBuddyAlreadyBuddyException() : base("The buddy you are adding is already in your buddy list!", GPErrorCode.AddBlockAlreadyBlocked)
        {
        }

        public GPAddBuddyAlreadyBuddyException(string message) : base(message, GPErrorCode.AddBuddyAlreadyBuddy)
        {
        }

        public GPAddBuddyAlreadyBuddyException(string message, System.Exception innerException) : base(message, GPErrorCode.AddBuddyAlreadyBuddy, innerException)
        {
        }
    }
}