using PresenceSearchPlayer.Entity.Enumerate;
using PresenceSearchPlayer.Entity.Exception.General;

namespace PresenceSearchPlayer.Entity.Exception.AddBuddy
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