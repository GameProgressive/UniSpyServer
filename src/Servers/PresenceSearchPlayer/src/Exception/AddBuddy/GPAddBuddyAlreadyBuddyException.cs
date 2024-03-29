using UniSpy.Server.PresenceSearchPlayer.Enumerate;
using UniSpy.Server.PresenceSearchPlayer.Exception.General;

namespace UniSpy.Server.PresenceSearchPlayer.Exception.AddBuddy
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