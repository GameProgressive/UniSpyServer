using UniSpy.Server.PresenceSearchPlayer.Entity.Enumerate;
using UniSpy.Server.PresenceSearchPlayer.Entity.Exception.General;

namespace UniSpy.Server.PresenceSearchPlayer.Entity.Exception.AddBuddy
{
    public class GPAddBuddyBadNewException : GPException
    {
        public GPAddBuddyBadNewException() : base("The buddy name provided is invalid!", GPErrorCode.AddBuddyBadNew)
        {
        }

        public GPAddBuddyBadNewException(string message) : base(message, GPErrorCode.AddBuddyBadNew)
        {
        }

        public GPAddBuddyBadNewException(string message, System.Exception innerException) : base(message, GPErrorCode.AddBuddyBadNew, innerException)
        {
        }
    }
}